using Microsoft.AspNetCore.SignalR;
using RealtimeChatRG.Core.Entities;
using RealtimeChatRG.Core.Services;
using System;
using System.Threading.Tasks;

namespace RealtimeChatRG.Web.Manager
{
    public class ChatHub: Hub
    {
        #region CTOR
        private readonly IMessageService _messageService;
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
        #endregion
        public Task SendMessage(string roomId, string user, string message)
        {
            _messageService.Add(new Message {
                 RoomId = Convert.ToInt32(roomId),
                 Username = user,
                 Text = message
            });
            return Clients.All.SendAsync("ReceiveMessage",roomId, user, message);
        }
    }
}
