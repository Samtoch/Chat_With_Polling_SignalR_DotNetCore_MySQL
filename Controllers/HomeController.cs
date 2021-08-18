using Chat_With_Polling_SignalR_DotNetCore_MySQL.Models;
using Chat_With_Polling_SignalR_DotNetCore_MySQL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_With_Polling_SignalR_DotNetCore_MySQL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IChatService _chatService;

        public HomeController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public IActionResult Login()
        {
            ViewBag.CHATPARTNER = new List<SelectListItem>() { 
                new SelectListItem { Text = "TOCHI", Value = "TOCHI" },
                new SelectListItem { Text = "UGONNA", Value = "UGONNA" },
                new SelectListItem { Text = "UCHENNA", Value = "UCHENNA" },
                new SelectListItem { Text = "SAMUE", Value = "SAMUE" }
            };
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login user)
        {
            if (user != null)
            {
                if (user.UserName != null)
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("ChatPartner", user.ChatPartner);
                    HttpContext.Session.SetString("ConnectionId", "SAMTECHJOHN20210808");
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Login");
        }

        public IActionResult Index(ChatMessage msg)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                var _msg = msg;
                string senderId = HttpContext.Session.GetString("UserName");
                string recipient = HttpContext.Session.GetString("ChatPartner"); //SELECTED CHAT PARTNER. HE OR SHE HAS TO LOGIN TO VIEW HIS CHATS
                _msg.Connectionid = _chatService.QueryConnectionId(senderId, recipient).Result;
                if (_msg.Connectionid == null)
                {
                    _msg.Connectionid = _chatService.GenerateConnectionId(senderId, recipient).Result;
                }
                var responseMsg = _chatService.QueryChatMessages(_msg.Connectionid).Result;
                ViewData["Messages"] = responseMsg;
                ViewData["ChatPartner"] = recipient;
                ViewData["Connectionid"] = _msg.Connectionid;
                ViewData["UserId"] = senderId;
                ViewData["UserName"] = senderId;
                

                if (msg.Message != null)
                {
                    _chatService.SaveMessage(_msg);
                }

                return View(); 
            }
            return RedirectToAction("Login");
        }

        public IActionResult GetMessages()
        {
            //string retUser, retMessage;
            var msg = new ChatMessage()
            {
                UserId = "",
                Connectionid = "SAMTECHJOHN20210808",
                Message = ""
            };

            //await _chatService.SaveMessage(msg);
            var response = _chatService.QueryChatMessages(msg.Connectionid).Result;

            return Json(response);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
