using Microsoft.AspNetCore.Mvc;
using RealtimeChatRG.Core.Services;

namespace RealtimeChatRG.Web.Controllers
{
    public class HomeController : Controller
    {
        #region CTOR
        private readonly IMessageService _messageService;
        private readonly IRoomService _roomService;
        public HomeController(IMessageService messageService, IRoomService roomService)
        {
            _messageService = messageService;
            _roomService = roomService;
        }
        #endregion
        public IActionResult Index()
        {
            var rooms = _roomService.List();
            ViewBag.rooms = rooms;
            return View();
        }
        [HttpPost]
        public IActionResult GetMessages([FromBody]int RoomId)
        {
            var messages = _messageService.GetMessages(RoomId);
            return Json(messages);
        }
    }
}
