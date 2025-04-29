using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEmailQueueService
{
    public class SecurityHelper
    {
        //// Anahtar ve IV boyutları AES için geçerli boyutlarda olmalıdır
        //private static string Key = "12345678901234567890123456789012";  // 32 byte (256 bit) anahtar
        //private static string IV = "1234567890123456";  // 16 byte IV (Initialization Vector)

        public static (string Key, string IV) LoadKeysFromFile()
        {
            // Anahtar ve IV dosyalarının doğru yolunu belirt
            string filePathKey = @"C:\Users\akindev\Desktop\Keys\key.txt";
            string filePathIV = @"C:\Users\akindev\Desktop\Keys\iv.txt";

            // Dosyadan anahtar ve IV'yi oku
            string key = File.ReadAllText(filePathKey).Trim();  // Trim ile boşluklardan kurtul
            string iv = File.ReadAllText(filePathIV).Trim();    // Trim ile boşluklardan kurtul

            return (key, iv);
        }

        public static string Decrypt(string encryptedText)
        {
            // Key ve IV'yi dosyadan oku
            var (key, iv) = LoadKeysFromFile();

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);  // Key'i byte dizisine çevir
                aes.IV = Encoding.UTF8.GetBytes(iv);    // IV'yi byte dizisine çevir

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);  // Şifrelenmiş metni byte dizisine çevir
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(decryptedBytes); // Çözülen veriyi döndür
            }
        }

        //public static string Encrypt(string text)
        //{
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(Key);
        //        aes.IV = Encoding.UTF8.GetBytes(IV);

        //        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        //        byte[] inputBytes = Encoding.UTF8.GetBytes(text);
        //        byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        //        return Convert.ToBase64String(encryptedBytes); // Şifrelenmiş veriyi base64 formatında döndür
        //    }
        //}

        //public static string Decrypt(string encryptedText)
        //{
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(Key);
        //        aes.IV = Encoding.UTF8.GetBytes(IV);

        //        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        //        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
        //        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        //        return Encoding.UTF8.GetString(decryptedBytes); // Çözülen veriyi döndür
        //    }
        //}
    }
}
