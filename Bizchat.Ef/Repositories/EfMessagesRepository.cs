using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;
using Bizchat.Core.Repositories;

namespace Bizchat.Ef.Repositories
{
    public class EfMessagesRepository : IMessagesRepository
    {
        private BizchatDbContext _db;

        public EfMessagesRepository(BizchatDbContext db)
        {
            _db = db;
        }

        public IQueryable<ChatMessage> List => _db.ChatMessages;

        public void Add(ChatMessage message)
        {
            _db.ChatMessages.Add(message);

            _db.SaveChanges();
        }
    }
}
