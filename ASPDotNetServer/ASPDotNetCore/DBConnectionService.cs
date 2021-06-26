using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace ASPDotNetCore
{
    public class DBConnectionService
    {
        string m_connectionString = null;

        public string ConnectionString { get { return m_connectionString; } }

        public DBConnectionService(string connectionString)
        {
            m_connectionString = connectionString;
        }

        private MySqlConnection getConnection()
        {
            return new MySqlConnection(m_connectionString);
        }

        public void ExcuteQuery(string query, Action<MySqlDataReader> readAction)
        {
            try
            {
                using (MySqlConnection mySqlConnection = getConnection())
                {
                    mySqlConnection.Open();

                    MySqlCommand sqlCommand = new MySqlCommand(query, mySqlConnection);
                    readAction(sqlCommand.ExecuteReader());
                    
                    mySqlConnection.Close();
                }
            }
            catch
            {

            }
        }
    }
}
