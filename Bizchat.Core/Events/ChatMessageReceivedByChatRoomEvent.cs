using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Events
{
    public class ChatMessageReceivedByChatRoomEvent
    {
        public int Id { get; set; }

        public ChatMessage ChatMessage { get; set; }

        public ChatRoom ChatRoom { get; set; }

        public DateTime DateReceived { get; set; }
    }
}
