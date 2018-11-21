using System;
using System.Collections.Generic;
using System.Text;

namespace Bizchat.Core.Entities
{
    public class ChatUser : IEquatable<ChatUser>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChatUser);
        }

        public bool Equals(ChatUser other)
        {
            return other != null &&
                   UserId == other.UserId &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, Name);
        }

        public static bool operator ==(ChatUser user1, ChatUser user2)
        {
            return EqualityComparer<ChatUser>.Default.Equals(user1, user2);
        }

        public static bool operator !=(ChatUser user1, ChatUser user2)
        {
            return !(user1 == user2);
        }
    }
}
