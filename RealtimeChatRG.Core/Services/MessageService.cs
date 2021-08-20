using RealtimeChatRG.Core.Entities;
using RealtimeChatRG.Core.Interfaces;
using System.Collections.Generic;

namespace RealtimeChatRG.Core.Services
{
    public class MessageService : IMessageService
    {
        #region CTOR
        private readonly IDatabaseProvider _db;
        public MessageService(IDatabaseProvider db)
        {
            _db = db;
        }
        #endregion
        public List<Message> GetMessages(int RoomId)
        {
            _db.AddParameter("@RoomId", RoomId);
            var result = _db.List<Message>("select * from Messages where RoomId=@RoomId order by Date");
            return result;
        }

        public void Add(Message entity)
        {
            _db.AddParameter("@RoomId", entity.RoomId);
            _db.AddParameter("@Username", entity.Username);
            _db.AddParameter("@Text", entity.Text);
            _db.Query("insert into Messages (RoomId,Username,Text) values (@RoomId,@Username,@Text)");
        }
    }

    public interface IMessageService
    {
        void Add(Message room);
        List<Message> GetMessages(int RoomId);
    }
}
