using Chat_With_Polling_SignalR_DotNetCore_MySQL.Models;
using Chat_With_Polling_SignalR_DotNetCore_MySQL.Repository;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_With_Polling_SignalR_DotNetCore_MySQL.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }
        public async Task SendMessage(string user, string message)
        {
            string retUser, retMessage;
            var msg = new ChatMessage() 
            {
                UserId = user, Connectionid = "SAMTECHJOHN20210808", Message = message
            };

            await _chatService.SaveMessage(msg);
            var response = await _chatService.QueryChatMessages(msg.Connectionid);
            foreach (var item in response)
            {
                retUser = item.UserId; retMessage = item.Message;
                await Clients.All.SendAsync("ReceiveMessage", retUser, retMessage);
            }
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }

    public class SignalrHub : Hub
    {
        private readonly IChatService _chatService;
        public SignalrHub(IChatService chatService)
        {
            _chatService = chatService;
        }
        public async Task SendMessage(string user, string userId, string connectionid, string message)
        {
            ChatMessage msg = new ChatMessage() { Message = message, UserId = userId, Connectionid = connectionid, Sender = user  };
            await _chatService.SaveMessage(msg);
            //messageRepository.Add(new Message { ChatId = Int32.Parse(chatId), Sender = Int32.Parse(userId), Content = message, SendTime = DateTime.Now });
            await Clients.All.SendAsync("ReceiveMessage", user, userId, connectionid, message);
        }
    }
}
