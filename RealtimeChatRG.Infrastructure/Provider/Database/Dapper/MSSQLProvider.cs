using Dapper;
using Microsoft.Extensions.Options;
using RealtimeChatRG.Core.ComplexType;
using RealtimeChatRG.Core.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RealtimeChatRG.Infrastructure.Provider.Database.Dapper
{
    public class MSSQLProvider : IDatabaseProvider
    {
        private SqlConnection _con;
        private readonly string _conStr;
        private DynamicParameters _parameters;
        public MSSQLProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            _conStr = connectionStrings.Value.Application;
            ClearParameters();
        }
        public void Open()
        {
            if (_con == null)
            {
                _con = new SqlConnection(_conStr);
            }
            if (_con.State != ConnectionState.Open)
            {
                _con.Open();
            }
        }
        public void Close()
        {
            _con.Close();
            ClearParameters();
        }

        public T Find<T>(string sql)
        {
            Open();
            sql = "set dateformat dmy;\r\n" + sql;
            T result;
            if (_parameters.ParameterNames.Count() == 0)
            {
                result = _con.Query<T>(sql).FirstOrDefault();
            }
            else
            {
                result = _con.Query<T>(sql, _parameters).FirstOrDefault();
            }
            Close();
            return result;
        }
        public List<T> List<T>(string sql)
        {
            Open();
            sql = "set dateformat dmy;\r\n" + sql;
            List<T> result;
            if (_parameters.ParameterNames.Count() == 0)
            {
                result = _con.Query<T>(sql).ToList();
            }
            else
            {
                result = _con.Query<T>(sql, _parameters).ToList();
            }
            Close();
            return result;
        }

        public void Query(string sql)
        {
            Open();
            sql = "set dateformat dmy;\r\n" + sql;
            if (_parameters.ParameterNames.Count() == 0)
            {
                _con.Query(sql);
            }
            else
            {
                _con.Query(sql, _parameters);
            }
            Close();
        }

        public int Execute(string sql)
        {
            int result = 0;
            Open();
            sql = "set dateformat dmy;\r\n" + sql;
            if (_parameters.ParameterNames.Count() == 0)
            {
                result = _con.Execute(sql);
            }
            else
            {
                result = _con.Execute(sql, _parameters);
            }
            Close();
            return result;
        }

        public void AddParameter(string key, object value, DbType? dbType = null, ParameterDirection? direction = null, int? size = null)
        {
            _parameters.Add(key, value, dbType, direction, size);
        }

        public void ClearParameters()
        {
            _parameters = new DynamicParameters();
        }

        public object ExecuteScalar(string sql)
        {

            Open();
            sql = "set dateformat dmy;\r\n" + sql;
            object result;
            if (_parameters.ParameterNames.Count() == 0)
            {
                result = _con.Execute(sql, commandType: System.Data.CommandType.StoredProcedure);
            }
            else
            {
                result = _con.Execute(sql, _parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            Close();
            return result;
        }

        public Dictionary<string, string> ExecuteReturnDictionary(string sql)
        {
            Open();
            object result;
            if (_parameters.ParameterNames.Count() == 0)
            {
                result = _con.Execute(sql, commandType: CommandType.StoredProcedure);
            }
            else
            {
                result = _con.Execute(sql, _parameters, commandType: CommandType.StoredProcedure);
            }
            Dictionary<string, string> outputs = new Dictionary<string, string>();

            foreach (var item in _parameters.ParameterNames.ToList())
            {
                try
                {
                    outputs.Add(item, _parameters.Get<string>(item));
                }
                catch
                {
                }
            }
            Close();
            return outputs;
        }

        public long Count(string sql)
        {
            throw new System.NotImplementedException();
        }

        public bool HasRow(string sql)
        {
            Open();
            sql = "set dateformat dmy;\r\n" + sql;
            bool result;
            if (_parameters.ParameterNames.Count() == 0)
            {
                result = _con.ExecuteScalar<bool>(sql, commandType: System.Data.CommandType.Text);
            }
            else
            {
                result = _con.ExecuteScalar<bool>(sql, _parameters, commandType: System.Data.CommandType.Text);
            }
            Close();
            return result;
        }
    }
}
