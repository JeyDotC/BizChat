using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;
using Bizchat.Core.Repositories;

namespace Bizchat.Ef.Repositories
{
    public class EfChatRoomsRepository : IChatRoomsRepository
    {
        private BizchatDbContext _db;

        public EfChatRoomsRepository(BizchatDbContext db)
        {
            _db = db;
        }

        public void Create(ChatUser owner, string name)
        {
            var chatRoom = new ChatRoom
            {
                Owner = owner,
                Title = name
            };

            _db.ChatRooms.Add(chatRoom);

            _db.SaveChanges();
        }

        public void AddMember(int id, ChatUser newUser)
        {
            var chatRoom = _db.ChatRooms.Find(id);

            _db.ChatRoomMemberships.Add(new Tables.ChatRoomMembership
            {
                ChatRoomId = chatRoom.Id,
                ChatRoom = chatRoom,
                ChatUserId = newUser.Id,
                ChatUser = newUser,
            });

            _db.SaveChanges();
        }

        public IEnumerable<ChatUser> ListMembers(int chatRoomId)
            => _db.ChatRoomMemberships.Where(m => m.ChatRoomId == chatRoomId).Select(m => m.ChatUser);

        public IQueryable<ChatRoom> List { get; }
    }
}
