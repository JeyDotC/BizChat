using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Ef.Tables
{
    public class ChatRoomReceivedMessages
    {
        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }

        public int ChatMessageId { get; set; }
        public ChatMessage ChatMessage { get; set; }

    }
}
