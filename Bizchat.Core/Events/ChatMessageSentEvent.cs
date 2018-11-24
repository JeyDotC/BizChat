using System;
using System.Collections.Generic;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Events
{
    public class ChatMessageSentEvent
    {
        public int Id { get; set; }

        public ChatMessage Contents { get; set; }

        public DateTime DateSent { get; set; } = DateTime.Now;
    }
}