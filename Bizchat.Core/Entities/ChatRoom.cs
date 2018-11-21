using System;
using System.Collections.Generic;
using System.Text;

namespace Bizchat.Core.Entities
{
    public class ChatRoom
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ChatUser Owner { get; set; }
    }
}
