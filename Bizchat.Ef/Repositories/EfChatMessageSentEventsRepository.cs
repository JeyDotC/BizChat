using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Events;
using Bizchat.Core.Repositories;
using Microsoft.EntityFrameworkCore;

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
            if (chatMessageSentEvent.Contents != null)
            {
                _db.Entry(chatMessageSentEvent.Contents).State = EntityState.Unchanged;

                if (chatMessageSentEvent.Contents.Sender != null)
                {
                    _db.Entry(chatMessageSentEvent.Contents.Sender).State = EntityState.Unchanged;
                }
            }

            _db.ChatMessageSentEvents.Add(chatMessageSentEvent);

            _db.SaveChanges();
        }
    }
}
