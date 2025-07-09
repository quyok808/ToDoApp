using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DbContext.Oracles
{
    public class OracleParamInfo
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public ParameterDirection ParameterDirection { get; set; }

        public OracleDbType? OracleDbMappingType { get; set; }

        public int? Size { get; set; }

        public IDbDataParameter AttachedParam { get; set; }

        public bool IsArray { get; set; }

        public int[] ArrayBindSize { get; set; }
    }
}
