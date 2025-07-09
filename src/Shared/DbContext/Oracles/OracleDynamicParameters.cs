using System.Data;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace DbContext.Oracles
{
    public class OracleDynamicParameters : SqlMapper.IDynamicParameters, IDisposable
    {
        private static readonly Dictionary<SqlMapper.Identity, Action<IDbCommand, object>> paramReaderCache = new Dictionary<SqlMapper.Identity, Action<IDbCommand, object>>();

        private readonly Dictionary<string, OracleParamInfo> parameters = new Dictionary<string, OracleParamInfo>();

        private List<object> templates;

        public OracleDynamicParameters()
        {
        }

        public OracleDynamicParameters(object template)
        {
            AddDynamicParams(template);
        }

        void SqlMapper.IDynamicParameters.AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            AddParameters(command, identity);
        }

        public OracleParameter GetParameter(string parameterName)
        {
            OracleParamInfo oracleParamInfo = parameters[Clean(parameterName)];
            return new OracleParameter
            {
                ParameterName = oracleParamInfo.Name,
                Value = oracleParamInfo.Value,
                OracleDbType = oracleParamInfo.OracleDbMappingType.Value,
                Direction = oracleParamInfo.ParameterDirection,
                Size = (oracleParamInfo.Size.HasValue ? oracleParamInfo.Size.Value : 4000)
            };
        }

        protected void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            LockParamReaderCache(command, identity);
            foreach (OracleParamInfo value in parameters.Values)
            {
                AddOracleParameter(command, value);
            }
        }

        private void LockParamReaderCache(IDbCommand command, SqlMapper.Identity identity)
        {
            if (templates == null)
            {
                return;
            }

            foreach (object template in templates)
            {
                SqlMapper.Identity identity2 = identity.ForDynamicParameters(template.GetType());
                Action<IDbCommand, object> value;
                lock (paramReaderCache)
                {
                    if (!paramReaderCache.TryGetValue(identity2, out value))
                    {
                        value = SqlMapper.CreateParamInfoGenerator(identity2, checkForDuplicates: false, removeUnused: false);
                        paramReaderCache[identity2] = value;
                    }
                }

                value(command, template);
            }
        }

        private static void AddOracleParameter(IDbCommand command, OracleParamInfo param)
        {
            string text = Clean(param.Name);
            bool num = !((OracleCommand)command).Parameters.Contains(text);
            OracleParameter oracleParameter;
            if (num)
            {
                oracleParameter = ((OracleCommand)command).CreateParameter();
                oracleParameter.ParameterName = text;
            }
            else
            {
                oracleParameter = ((OracleCommand)command).Parameters[text];
            }

            object value = param.Value;
            oracleParameter.Value = value ?? DBNull.Value;
            oracleParameter.Direction = param.ParameterDirection;
            if (value is string text2 && text2.Length <= 4000)
            {
                oracleParameter.Size = 4000;
            }

            if (param.Size.HasValue)
            {
                oracleParameter.Size = param.Size.Value;
            }

            if (param.OracleDbMappingType.HasValue)
            {
                oracleParameter.OracleDbType = param.OracleDbMappingType.Value;
            }

            if (num)
            {
                command.Parameters.Add(oracleParameter);
                if (param.IsArray)
                {
                    oracleParameter.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                }

                param.AttachedParam = oracleParameter;
            }
        }

        public void AddDynamicParams(dynamic param)
        {
            if ((object)param == null)
            {
                return;
            }

            if (!(param is OracleDynamicParameters oracleDynamicParameters))
            {
                if (!(param is IEnumerable<KeyValuePair<string, object>> enumerable))
                {
                    if (templates == null)
                    {
                        templates = new List<object>();
                    }

                    templates.Add((object)param);
                    return;
                }

                {
                    foreach (KeyValuePair<string, object> item in enumerable)
                    {
                        Add(item.Key, item.Value);
                    }

                    return;
                }
            }

            if (oracleDynamicParameters.parameters != null)
            {
                foreach (KeyValuePair<string, OracleParamInfo> parameter in oracleDynamicParameters.parameters)
                {
                    parameters.Add(parameter.Key, parameter.Value);
                }

                return;
            }

            if (oracleDynamicParameters.templates != null)
            {
                if (templates == null)
                {
                    templates = new List<object>();
                }

                templates.AddRange(oracleDynamicParameters.templates);
            }
        }

        public void Add(string name, object value = null, OracleDbType? oracleDbMappingType = null, ParameterDirection? direction = null, int? size = null)
        {
            parameters[Clean(name)] = new OracleParamInfo
            {
                Name = Clean(name),
                Value = value,
                ParameterDirection = (direction ?? ParameterDirection.Input),
                OracleDbMappingType = oracleDbMappingType,
                Size = size
            };
        }

        public void AddAssociativeArray(string name, object value, int size, OracleDbType? oracleDbMappingType = null, ParameterDirection? direction = null)
        {
            parameters[Clean(name)] = new OracleParamInfo
            {
                Name = Clean(name),
                Value = value,
                ParameterDirection = (direction ?? ParameterDirection.Input),
                OracleDbMappingType = oracleDbMappingType,
                Size = size,
                IsArray = true
            };
        }

        public void AddCursor(string name)
        {
            parameters[Clean(name)] = new OracleParamInfo
            {
                Name = name,
                Value = DBNull.Value,
                ParameterDirection = ParameterDirection.Output,
                OracleDbMappingType = OracleDbType.RefCursor
            };
        }

        public T Get<T>(string name)
        {
            object value = parameters[Clean(name)].AttachedParam.Value;
            if (value == DBNull.Value && default(T) == null)
            {
                return default(T);
            }

            return (T)value;
        }

        public object GetParameterValue(string name)
        {
            object value = parameters[Clean(name)].AttachedParam.Value;
            OracleDbType value2 = parameters[Clean(name)].OracleDbMappingType.Value;
            if (value.IsOracleDecimalArray(out var outOracleDecimalValues))
            {
                return ParamValueIsDecimalArray(name, value2, outOracleDecimalValues);
            }

            if (value.IsOracleStringArray(out var outOracleStringValues))
            {
                return outOracleStringValues.Select((OracleString p) => p.IsNull ? string.Empty : p.Value).ToArray();
            }

            string text = value?.ToString();
            if (value.IsNullOrEmptyOfOracleValue())
            {
                return null;
            }

            if (value2.IsInt16AndInt32())
            {
                if (!int.TryParse(text, out var result))
                {
                    return null;
                }

                return result;
            }

            if (value2.IsInt64())
            {
                if (!long.TryParse(text, out var result2))
                {
                    return null;
                }

                return result2;
            }

            if (value2.IsDecimal())
            {
                if (!decimal.TryParse(text, out var result3))
                {
                    return null;
                }

                return result3;
            }

            if (value2.IsBinaryDoubleAndBinaryFloat())
            {
                if (!double.TryParse(text, out var result4))
                {
                    return null;
                }

                return result4;
            }

            return text;
        }

        private object ParamValueIsDecimalArray(string name, OracleDbType oracleParameter, OracleDecimal[] oracleDecimalValues)
        {
            IEnumerable<decimal> enumerable = oracleDecimalValues.Select((OracleDecimal p) => p.Value);
            if (enumerable.IsNullOrEmptyOfOracleValue())
            {
                return enumerable;
            }

            if (oracleParameter.IsInt16AndInt32())
            {
                return enumerable.Cast<int>();
            }

            if (oracleParameter.IsInt64())
            {
                return enumerable.Cast<long>();
            }

            if (oracleParameter.IsDecimal())
            {
                return enumerable.Reverse();
            }

            if (oracleParameter.IsBinaryDoubleAndBinaryFloat())
            {
                return enumerable.Cast<double>();
            }

            return (object[])parameters[Clean(name)].AttachedParam.Value;
        }

        private static string Clean(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                char c = name[0];
                if (c == ':' || c == '?' || c == '@')
                {
                    return name.Substring(1, name.Length - 1);
                }
            }

            return name;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dictionary<SqlMapper.Identity, Action<IDbCommand, object>> dictionary = paramReaderCache;
                if (dictionary != null && dictionary.Count > 0)
                {
                    paramReaderCache.Clear();
                }

                Dictionary<string, OracleParamInfo> dictionary2 = parameters;
                if (dictionary2 != null && dictionary2.Count > 0)
                {
                    parameters.Clear();
                }

                List<object> list = templates;
                if (list != null && list.Count > 0)
                {
                    templates.Clear();
                }
            }
        }

        ~OracleDynamicParameters()
        {
            Dispose(disposing: false);
        }
    }
}
