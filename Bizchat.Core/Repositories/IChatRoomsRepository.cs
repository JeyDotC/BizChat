using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Repositories
{
    public interface IChatRoomsRepository
    {
        ChatRoom Create(ChatUser owner, string name);

        void AddMember(int id, ChatUser newUser);

        ChatRoom Find(int id);

        IQueryable<ChatRoom> List { get; }

        IQueryable<ChatRoom> ListWhereUserIsMember(ChatUser member);

        IEnumerable<ChatUser> ListMembers(int chatRoomId);

        IEnumerable<ChatMessage> ListReceivedMessages(int chatRoomId);

        void ReceiveMessage(int chatRoomId, ChatMessage message);
    }
}
