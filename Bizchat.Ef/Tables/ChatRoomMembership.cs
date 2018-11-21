using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Ef.Tables
{
    public class ChatRoomMembership
    {
        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }

        public int ChatUserId { get; set; }
        public ChatUser ChatUser { get; set; }
    }
}
