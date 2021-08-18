using Chat_With_Polling_SignalR_DotNetCore_MySQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_With_Polling_SignalR_DotNetCore_MySQL.Repository
{
    public interface IChatService
    {
        Task<bool> SaveMessage(ChatMessage msg);
        Task<List<ChatMessage>> QueryChatMessages(string connectionId);
        Task<string> QueryConnectionId(string senderId, string receiverId);
        Task<string> GenerateConnectionId(string senderId, string receiverId);
    }
}
