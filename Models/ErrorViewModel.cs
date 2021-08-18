using System;

namespace Chat_With_Polling_SignalR_DotNetCore_MySQL.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
