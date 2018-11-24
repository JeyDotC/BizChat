using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Repositories
{
    public interface IChatMessagesRepository
    {
        ChatMessage Find(int id);

        IQueryable<ChatMessage> List { get; }

        void Add(ChatMessage message);

        void Update(ChatMessage message);
    }
}
