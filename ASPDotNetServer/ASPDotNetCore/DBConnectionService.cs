using MySql.Data.MySqlClient;
using System;

namespace ASPDotNetCore
{
    public class DBConnectionService
    {
        MySqlConnection m_mySqlConnection = null;

        public DBConnectionService(string connectionString)
        {
            m_mySqlConnection = new MySqlConnection(connectionString);
            m_mySqlConnection.Open();
        }

        public void ExcuteQuery(string query, Action<MySqlDataReader> readAction)
        {
            try
            {
                Log.Write("[{0}] {1} 실행...", m_mySqlConnection.Database, query);

                using (MySqlCommand sqlCommand = new MySqlCommand(query, m_mySqlConnection))
                {
                    using (MySqlDataReader mySqlDataReader = sqlCommand.ExecuteReader())
                    {

                        Log.Write("[{0}] {1} 실행 완료...", m_mySqlConnection.Database, query);

                        readAction(mySqlDataReader);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
