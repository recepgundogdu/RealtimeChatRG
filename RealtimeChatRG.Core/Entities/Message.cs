using System;

namespace RealtimeChatRG.Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
