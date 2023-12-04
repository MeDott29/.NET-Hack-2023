using System;

namespace NET_Hack_2023.Models
{
    public class ChatMessage
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
