using Microsoft.Extensions.DependencyInjection;

namespace ASPDotNetCore
{
    public class DBConnectionServiceDescriptor : ServiceDescriptor
    {
        DBConnectionService m_dbConnectionService = null;

        public DBConnectionService DBConnectionService { get { return m_dbConnectionService; } }

        public DBConnectionServiceDescriptor(string connectionString) : base(typeof(DBConnectionService), new DBConnectionService(connectionString))
        {
            m_dbConnectionService = ImplementationInstance as DBConnectionService;
        }
    }
}
