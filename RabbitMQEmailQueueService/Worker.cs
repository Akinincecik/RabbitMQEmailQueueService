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
    public class Worker
    {
        private static Object _lock = new Object();
        private static DatabaseParameters _dbParams = new DatabaseParameters();  // ✅ Bağlantı bilgilerini merkezi olarak çekiyoruz.

        public static void testMethod(object arg)
        {
            ThreadParameters threadParameters = (ThreadParameters)arg;
            List<string> databases = new List<string> { "SQL"/*, "PostgreSQL"*/ }; // ✅ Veritabanı türlerini listeye alıyoruz.

            while (true)
            {
                try
                {
                    lock (_lock)
                    {
                        foreach (var dbType in databases)
                        {
                            EmailQueueService.ProcessEmailQueue();
                            EmailQueueService.RetryFailedEmails(dbType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }

                Thread.Sleep(1000);
            }
        }

        private static void LogError(Exception ex)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("WindowsServiceTest Error: " + ex.StackTrace, EventLogEntryType.Error, 101, 1);
            }
        }
    }
}
