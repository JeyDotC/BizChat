using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;
using Bizchat.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bizchat.Ef.Repositories
{
    public class EfChatMessagesRepository : IChatMessagesRepository
    {
        private readonly BizchatDbContext _db;

        public EfChatMessagesRepository(BizchatDbContext db)
        {
            _db = db;
        }

        public ChatMessage Find(int id)
            => _db.ChatMessages.Include(m => m.Sender).FirstOrDefault(m => m.Id == id);

        public IQueryable<ChatMessage> List => _db.ChatMessages;

        public void Add(ChatMessage message)
        {
            _db.ChatMessages.Add(message);

            _db.SaveChanges();
        }

        public void Update(ChatMessage message)
        {
            _db.ChatMessages.Update(message);

            _db.UpdateRange();
        }
    }
}
