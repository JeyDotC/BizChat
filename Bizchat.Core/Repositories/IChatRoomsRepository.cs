using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Repositories
{
    public interface IChatRoomsRepository
    {
        void Create(ChatUser owner, string name);

        void AddMember(int id, ChatUser newUser);

        IQueryable<ChatRoom> List { get; }

        IEnumerable<ChatUser> ListMembers(int chatRoomId);
    }
}
