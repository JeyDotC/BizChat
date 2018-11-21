using System;
using System.Collections.Generic;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Services
{
    public class MessageToQueue : IEquatable<MessageToQueue>
    {
        public string RoutingKey { get; set; }

        public ChatMessage Contents { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as MessageToQueue);
        }

        public bool Equals(MessageToQueue other)
        {
            return other != null &&
                   RoutingKey == other.RoutingKey &&
                   EqualityComparer<ChatMessage>.Default.Equals(Contents, other.Contents);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RoutingKey, Contents);
        }

        public static bool operator ==(MessageToQueue queue1, MessageToQueue queue2)
        {
            return EqualityComparer<MessageToQueue>.Default.Equals(queue1, queue2);
        }

        public static bool operator !=(MessageToQueue queue1, MessageToQueue queue2)
        {
            return !(queue1 == queue2);
        }
    }
}