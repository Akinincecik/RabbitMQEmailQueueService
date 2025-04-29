using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEmailQueueService
{
    public static class ConnectionStringManager
    {
        public static string etDecryptedConnectionString()
        {
            string encryptedConnectionString = ConfigurationManager.ConnectionStrings["MsSqlConnection"].ConnectionString;

            string decryptedConnectionString = SecurityHelper.Decrypt(encryptedConnectionString);

            return decryptedConnectionString;
        }
    }
}
