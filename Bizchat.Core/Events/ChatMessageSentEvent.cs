using System;
using System.Collections.Generic;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Events
{
    public class ChatMessageSentEvent : IEquatable<ChatMessageSentEvent>
    {
        public string RoutingKey { get; set; }

        public ChatMessage Contents { get; set; }

        public object ExtraInfo { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChatMessageSentEvent);
        }

        public bool Equals(ChatMessageSentEvent other)
        {
            return other != null &&
                   RoutingKey == other.RoutingKey &&
                   EqualityComparer<ChatMessage>.Default.Equals(Contents, other.Contents);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RoutingKey, Contents);
        }

        public static bool operator ==(ChatMessageSentEvent queue1, ChatMessageSentEvent queue2)
        {
            return EqualityComparer<ChatMessageSentEvent>.Default.Equals(queue1, queue2);
        }

        public static bool operator !=(ChatMessageSentEvent queue1, ChatMessageSentEvent queue2)
        {
            return !(queue1 == queue2);
        }
    }
}