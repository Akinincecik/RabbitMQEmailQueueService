using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEmailQueueService
{
    public class SQLUtils
    {
        //private static readonly string _sqlConnectionString = DatabaseParameters.GetConnectionString("MsSqlConnection");

        private static readonly string _sqlConnectionString = ConnectionStringManager.etDecryptedConnectionString();

        public static DataTable GetSqlQueuedEmails()
        {
            return ExecuteSqlStoredProcedure("GetSqlQueuedEmails");
        }

        public static DataTable GetEmailFromId(int emailId)
        {
            return ExecuteSqlStoredProcedure("GetEmailFromId", new SqlParameter("@EmailId", emailId));
        }

        public static DataTable UpdateSqlEmailStatus(int emailId, int result)
        {
            return ExecuteSqlStoredProcedure("UpdateSqlEmailStatus", new SqlParameter("@EmailId", emailId), new SqlParameter("@Result", result));
        }

        public static DataTable GetSqlEmailsByStatus(int state)
        {
            return ExecuteSqlStoredProcedure("GetEmailByStatus", new SqlParameter("@State", state));
        }

        private static DataTable ExecuteSqlStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);

                    return ExecuteSqlCommand(cmd, connection);
                }

            }
        }

        private static DataTable ExecuteSqlCommand(SqlCommand cmd, SqlConnection connection)
        {
            connection.Open();
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
