using System.Data;

namespace DbContext.Connections
{
    public interface IConnection
    {
        IDbConnection OracleConnection(string connectionString);
        void DbConnectionClose(IDbConnection dbConnection);
    }
}
