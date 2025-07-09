using Oracle.ManagedDataAccess.Client;

namespace DbContext.Oracles
{
    public static class CheckOracleDbType
    {
        public static bool IsInt16AndInt32(this OracleDbType oracleDbType)
        {
            return oracleDbType switch
            {
                OracleDbType.Int16 => true,
                OracleDbType.Int32 => true,
                _ => false,
            };
        }

        public static bool IsInt64(this OracleDbType oracleDbType)
        {
            if (oracleDbType == OracleDbType.Int64)
            {
                return true;
            }

            return false;
        }

        public static bool IsDecimal(this OracleDbType oracleDbType)
        {
            if (oracleDbType == OracleDbType.Decimal)
            {
                return true;
            }

            return false;
        }

        public static bool IsBinaryDoubleAndBinaryFloat(this OracleDbType oracleDbType)
        {
            return oracleDbType switch
            {
                OracleDbType.BinaryDouble => true,
                OracleDbType.BinaryFloat => true,
                _ => false,
            };
        }
    }
}
