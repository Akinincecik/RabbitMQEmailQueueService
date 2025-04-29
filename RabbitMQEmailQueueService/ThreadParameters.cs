using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEmailQueueService
{
    public class ThreadParameters
    {
        public string ThreadName { get; set; }
        public string ConnectionString { get; set; }

        public DatabaseParameters DbParams { get; set; } // ✅ Veritabanı bilgileri burada tutuluyor

    }
}
