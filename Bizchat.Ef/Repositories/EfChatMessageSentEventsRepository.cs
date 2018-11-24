using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Events;
using Bizchat.Core.Repositories;

namespace Bizchat.Ef.Repositories
{
    public class EfChatMessageSentEventsRepository : IChatMessageSentEventsRepository
    {
        private readonly BizchatDbContext _db;

        public EfChatMessageSentEventsRepository(BizchatDbContext db)
        {
            _db = db;
        }

        public void Add(ChatMessageSentEvent chatMessageSentEvent)
        {
            _db.ChatMessageSentEvents.Add(chatMessageSentEvent);

            _db.SaveChanges();
        }
    }
}
