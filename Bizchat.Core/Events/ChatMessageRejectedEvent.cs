using System;
using System.Collections.Generic;
using System.Text;

namespace Bizchat.Core.Events
{
    public class ChatMessageRejectedEvent
    {
        public int Id { get; set; }

        public ChatMessageSentEvent OriginalMessage { get; set; }

        public string Reason { get; set; }

        public DateTime DateRejected { get; set; }
    }
}
