using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEmailQueueService
{
    public class Utils
    {
        public static bool IsValidJson(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                return false;

            try
            {
                JToken.Parse(jsonData);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
