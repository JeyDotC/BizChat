using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;
using Bizchat.Core.Repositories;

namespace Bizchat.Ef.Repositories
{
    public class EfChatUsersRepository : IChatUsersRepository
    {
        private readonly BizchatDbContext _db;

        public EfChatUsersRepository(BizchatDbContext db)
        {
            _db = db;
        }

        public ChatUser Find(int id)
         => _db.ChatUsers.Find(id);

        public void Add(ChatUser chatUser)
        {
            _db.ChatUsers.Add(chatUser);

            _db.SaveChanges();
        }

        public IQueryable<ChatUser> List => _db.ChatUsers;
    }
}
