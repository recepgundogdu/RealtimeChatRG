using System.Collections.Generic;
using System.Data;

namespace RealtimeChatRG.Core.Interfaces
{
    public interface IDatabaseProvider
    {
        void Query(string query);
        int Execute(string query);
        T Find<T>(string query);
        List<T> List<T>(string query);
        void Close();
        void Open();
        long Count(string query);
        bool HasRow(string query);
        object ExecuteScalar(string query);
        Dictionary<string, string> ExecuteReturnDictionary(string query);
        void AddParameter(string key, object value, DbType? dbType = null, ParameterDirection? direction = null, int? size = null);
        void ClearParameters();
    }
}
