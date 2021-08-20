using System.Collections.Generic;

namespace RealtimeChatRG.Core.Interfaces
{
    public interface ICacheProvider
    {
        bool Exists(string key);
        T Get<T>(string key);
        void Clear();
        void Remove(string key);
        void Add<T>(string key, List<T> data, int duration = 60);
        void Add<T>(string key, T data, int duration = 60);
    }
}
