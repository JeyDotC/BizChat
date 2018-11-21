using System;
using System.Collections.Generic;

namespace Bizchat.Core.Entities
{
    public class ChatMessage : IEquatable<ChatMessage>
    {
        public int Id { get; set; }

        public ChatUser Sender { get; set; }

        public string Contents { get; set; }

        public string Destination { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChatMessage);
        }

        public bool Equals(ChatMessage other)
        {
            return other != null &&
                   Id == other.Id &&
                   Sender == other.Sender &&
                   Contents == other.Contents &&
                   Destination == other.Destination;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Sender, Contents, Destination);
        }

        public static bool operator ==(ChatMessage message1, ChatMessage message2)
        {
            return EqualityComparer<ChatMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ChatMessage message1, ChatMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
