using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Events
{
    public class ChatMessageReceivedEvent
    {
        public ChatMessage ChatMessage { get; set; }
    }
}
