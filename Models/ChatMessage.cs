using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_With_Polling_SignalR_DotNetCore_MySQL.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime Logtime { get; set; }
        public string Connectionid { get; set; }
        public bool Read { get; set; }
        public string Sender { get; set; } 
        public string Del_Flg { get; set; }
    }
}
