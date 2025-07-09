using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace DbContext.Connections
{
    public class Connection : IConnection
    {
        private readonly IConfiguration configuration;

        public Connection(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void DbConnectionClose(IDbConnection dbConnection)
        {
            if (dbConnection.State == ConnectionState.Open || dbConnection.State == ConnectionState.Broken)
            {
                dbConnection.Close();
            }
        }

        public IDbConnection OracleConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }

    }
}
