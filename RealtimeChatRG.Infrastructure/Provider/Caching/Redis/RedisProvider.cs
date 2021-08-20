using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RealtimeChatRG.Core.ComplexType;
using RealtimeChatRG.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace RealtimeChatRG.Infrastructure.Provider.Caching.Redis
{
    public class RedisProvider : ICacheProvider
    {
        #region CTOR
        private readonly IDatabase _database;
        public RedisProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            _database = ConnectionMultiplexer.Connect(connectionStrings.Value.Redis).GetDatabase();
        }
        #endregion

        public string[] Keys()
        {
            var keys = _database.Execute("KEYS", "*");
            if (!keys.IsNull)
            {
                return (string[])keys;
            }

            return Array.Empty<string>();
        }
        public bool Exists(string key)
        {
            return _database.KeyExists(key);
        }


        public T Get<T>(string key)
        {
            var data = _database.StringGet(key);
            if (data.HasValue)
            {
                return JsonConvert.DeserializeObject<T>((string)data);
            }

            return default(T);
        }
        public T Get<T>(string key, Func<T> acquire, int duration = 60)
        {
            if (Exists(key))
            {
                return Get<T>(key);
            }

            return Set(key, acquire(), duration);
        }
        public T Set<T>(string key, T data, int duration = 60)
        {
            Add(key, data, duration);
            return data;
        }


        public void Clear()
        {
            _database.Execute("FLUSHDB");
        }
        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }
        public void RemoveByPattern(string pattern)
        {
            var keys = _database.Execute("KEYS", pattern);
            if (!keys.IsNull)
            {
                _database.Execute("DEL", (string[])keys);
            }
        }

        public void Add<T>(string key, List<T> data, int duration = 60)
        {
            if (data == null && data.Count != 0)
            {
                return;
            }

            _database.StringSetAsync(key, JsonConvert.SerializeObject(data), TimeSpan.FromMinutes(duration));
        }

        public void Add<T>(string key, T data, int duration = 60)
        {
            if (data == null)
            {
                return;
            }

            _database.StringSetAsync(key, JsonConvert.SerializeObject(data), TimeSpan.FromMinutes(duration));
        }
    }
}