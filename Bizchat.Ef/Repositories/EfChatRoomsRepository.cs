﻿using System;
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

        public ChatRoom Create(ChatUser owner, string name)
        {
            var chatRoom = new ChatRoom
            {
                Owner = owner,
                Title = name
            };

            _db.ChatRooms.Add(chatRoom);

            _db.SaveChanges();

            return chatRoom;
        }

        public void AddMember(int id, ChatUser newUser)
        {
            var chatRoom = Find(id);

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

        public IQueryable<ChatRoom> ListWhereUserIsMember(ChatUser member)
            => _db.ChatRoomMemberships.Where(m => m.ChatUserId == member.Id).Select(m => m.ChatRoom);

        public IEnumerable<ChatMessage> ListReceivedMessages(int chatRoomId)
            => _db.ChatRoomReceivedMessages.Where(r => r.ChatRoomId == chatRoomId).Select(r => r.ChatMessage);

        public void ReceiveMessage(int chatRoomId, ChatMessage message)
        {
            var chatRoom = Find(chatRoomId);

            _db.ChatRoomReceivedMessages.Add(new Tables.ChatRoomReceivedMessages
            {
                ChatMessage = message,
                ChatMessageId = message.Id,
                ChatRoom = chatRoom,
                ChatRoomId = chatRoom.Id
            });

            _db.SaveChanges();
        }

        public IQueryable<ChatRoom> List
            => _db.ChatRooms;

        public ChatRoom Find(int id)
            => _db.ChatRooms.Find(id) ?? throw new InvalidOperationException("Given chat room id doesn't exist");
    }
}
