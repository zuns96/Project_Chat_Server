using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ASPDotNetCore
{
    public enum E_DB : byte
    {
        E_DB_ACCOUNT = 0,
        E_DB_CHAT,
        E_DB_LOG,
    };

    public class DBManager
    {
        static DBManager s_instance = null;

        static DBConnectionServiceDescriptor s_dbAccount = null;
        static DBConnectionServiceDescriptor s_dbChat = null;
        static DBConnectionServiceDescriptor s_dbLog = null;

        static public void Create()
        {
            if(s_instance == null)
            {
                s_instance = new DBManager();
            }
        }

        static public void Release()
        {
            s_instance = null;
        }

        static public List<ServiceDescriptor> Init(IConfiguration configuration)
        {
            if (s_instance != null)
                return s_instance.init(configuration);
            return new List<ServiceDescriptor>();
        }

        static public void ExcuteQuery(E_DB eDB, string query, Action<MySqlDataReader> readAction)
        {
            if (s_instance != null)
                s_instance.excuteQuery(eDB, query, readAction);
        }

        DBConnectionService getDBConn(E_DB eDB)
        {
            switch(eDB)
            {
                case E_DB.E_DB_ACCOUNT:
                    {
                        return s_dbAccount.DBConnectionService;
                    }
                case E_DB.E_DB_CHAT:
                    {
                        return s_dbChat.DBConnectionService;
                    }
                case E_DB.E_DB_LOG:
                    {
                        return s_dbLog.DBConnectionService;
                    }
                default:
                    {
                        return s_dbAccount.DBConnectionService;
                    }
            }
        }

        List<ServiceDescriptor> init(IConfiguration configuration)
        {
            List<ServiceDescriptor> lst = new List<ServiceDescriptor>();

            s_dbAccount = new DBConnectionServiceDescriptor(configuration.GetConnectionString("ChatDB_Account"));
            lst.Add(s_dbAccount);

            s_dbChat = new DBConnectionServiceDescriptor(configuration.GetConnectionString("ChatDB_Chat"));
            lst.Add(s_dbChat);

            s_dbLog = new DBConnectionServiceDescriptor(configuration.GetConnectionString("ChatDB_Log"));
            lst.Add(s_dbLog);

            return lst;
        }

        private void excuteQuery(E_DB eDB, string query, Action<MySqlDataReader> readAction)
        {
            DBConnectionService dbConn = getDBConn(eDB);
            lock (dbConn)
            {
                dbConn.ExcuteQuery(query, readAction);
            }
        }
    }
}
