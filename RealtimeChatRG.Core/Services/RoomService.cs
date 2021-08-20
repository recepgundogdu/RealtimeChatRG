using RealtimeChatRG.Core.Entities;
using RealtimeChatRG.Core.Interfaces;
using System.Collections.Generic;

namespace RealtimeChatRG.Core.Services
{
    public class RoomService : IRoomService
    {
        #region CTOR
        private readonly IDatabaseProvider _db;
        private readonly ICacheProvider _cache;
        public RoomService(IDatabaseProvider db, ICacheProvider cache)
        {
            _db = db;
            _cache = cache;
        }
        #endregion
        public List<Room> List()
        {
            if (_cache.Exists("Rooms"))
            {
                return _cache.Get<List<Room>>("Rooms");
            }
            var result = _db.List<Room>("select * from Rooms order by RoomName");
            _cache.Add<List<Room>>("Rooms", result);
            return result;
        }
    }
    public interface IRoomService
    {
        List<Room> List();
    }
}
