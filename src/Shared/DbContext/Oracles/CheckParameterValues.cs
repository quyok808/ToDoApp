using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Types;

namespace DbContext.Oracles
{
    public static class CheckParameterValues
    {
        public static bool IsOracleDecimalArray(this object values, out OracleDecimal[] outOracleDecimalValues)
        {
            if (values is Array && values is OracleDecimal[] array)
            {
                outOracleDecimalValues = array;
                return true;
            }

            outOracleDecimalValues = null;
            return false;
        }

        public static bool IsOracleStringArray(this object values, out OracleString[] outOracleStringValues)
        {
            if (values is Array && values is OracleString[] array)
            {
                outOracleStringValues = array;
                return true;
            }

            outOracleStringValues = null;
            return false;
        }

        public static bool IsNullOrEmptyOfOracleValue(this object paramValue)
        {
            if (paramValue != null && paramValue != DBNull.Value)
            {
                return paramValue?.ToString()?.ToLower() == "null".ToLower();
            }

            return true;
        }
    }
}
