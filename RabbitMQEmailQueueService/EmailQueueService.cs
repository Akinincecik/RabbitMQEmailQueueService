using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQEmailQueueService
{
    public class EmailQueueService
    {
        public static void ProcessEmailQueue()
        {
            Console.WriteLine($"🔍 Yeni kayıtlar kontrol ediliyor...");

            // StatusId = 0 olan e-mailleri al
            DataTable newEmails = SQLUtils.GetSqlQueuedEmails();

            foreach (DataRow row in newEmails.Rows)
            {
                int emailId = Convert.ToInt32(row["Id"]);

                // Her bir emailId için detaylı veriyi al
                DataTable emailDetails = SQLUtils.GetEmailFromId(emailId);

                if (emailDetails.Rows.Count > 0)
                {
                    // Veritabanından gelen veriyi uygun formata getir
                    var emailData = emailDetails.Rows[0];

                    var emailParams = new Dictionary<string, object>
                    {
                        { "@Recipient", emailData["Recipient"] },
                        { "@Subject", emailData["Subject"] },
                        { "@Body", emailData["Body"] },
                        { "@JsonData", emailData["JsonData"] }, // JSON formatındaki veri
                        { "@HtmlData", emailData["HtmlData"] },  // HTML formatındaki veri
                        { "@EmailId", emailData["Id"] }         // EmailId parametresi ekleyin
                    };

                    // JSON'a çevir
                    string jsonData = JsonConvert.SerializeObject(emailParams);

                    try
                    {
                        // RabbitMQ'ya gönder
                        RabbitMQHelper.SendMessage(jsonData);

                        // Başarıyla RabbitMQ'ya gönderildiyse durumu 6 (Sent) yap
                        SQLUtils.UpdateSqlEmailStatus(emailId, 6);
                        Console.WriteLine($"✅ Email ID {emailId} RabbitMQ'ya başarıyla gönderildi.");
                    }
                    catch (Exception ex)
                    {
                        // RabbitMQ'ya gönderilemediyse durumu 4 (Failed) yap
                        SQLUtils.UpdateSqlEmailStatus(emailId, 4);
                        Console.WriteLine($"❌ Email ID {emailId}: RabbitMQ gönderimi başarısız! {ex.Message}");
                    }
                }
            }
        }

        public static void RetryFailedEmails(string dbType)
        {
            foreach (int status in new[] { 1, 4 }) // 1: İşlemde kalan, 4: Başarısız olan
            {
                DataTable emails = GetEmailsByStatus(status);

                foreach (DataRow row in emails.Rows)
                {
                    int emailId = Convert.ToInt32(row["Id"]);
                    string jsonData = row["JsonData"].ToString();

                    switch (status)
                    {
                        case 1: // İşlemde kalan mailler
                            if (!Utils.IsValidJson(jsonData))
                            {
                                Console.WriteLine($"❌ Email ID {emailId}: JSON Hatalı! ({dbType})");
                                UpdateEmailStatus(dbType, emailId, 5); // JSON hatalıysa tekrar denenmesin
                            }
                            else
                            {
                                Console.WriteLine($"✅ Email ID {emailId}: Tekrar kuyruğa alındı! ({dbType})");
                                UpdateEmailStatus(dbType, emailId, 0); // Tekrar işleme al (Queued)
                            }
                            break;

                        case 4: // Başarısız olan mailler
                            if (!Utils.IsValidJson(jsonData))
                            {
                                Console.WriteLine($"❌ Email ID {emailId}: JSON Hatalı! ({dbType})");
                                UpdateEmailStatus(dbType, emailId, 5); // JSON hatalıysa tekrar denenmesin
                            }
                            else
                            {
                                Console.WriteLine($"✅ Email ID {emailId}: Tekrar kuyruğa alındı! ({dbType})");
                                UpdateEmailStatus(dbType, emailId, 0); // Tekrar işleme al (Queued)
                            }
                            break;

                        default:
                            Console.WriteLine($"⚠️ Bilinmeyen email durumu: {status} (Email ID: {emailId})");
                            break;
                    }
                }
            }
        }

        //private static DataTable GetEmailsByStatus(string dbType, int status) =>
        //    dbType == "SQL" ? SQLUtils.GetEmailsByStatus(status) : SQLUtils.GetPostgreEmailsByStatus(status);

        private static DataTable GetEmailsByStatus(int status) =>
            SQLUtils.GetSqlEmailsByStatus(status);


        private static void UpdateEmailStatus(string dbType, int emailId, int status)
        {
            if (dbType == "SQL")
            {
                SQLUtils.UpdateSqlEmailStatus(emailId, status);
            }
            //else if (dbType == "PostgreSQL")
            //{
            //    SQLUtils.UpdatePostgreEmailStatus(emailId, status);
            //}
            else
            {
                throw new ArgumentException($"Geçersiz veritabanı tipi: {dbType}");
            }
        }
    }
}
