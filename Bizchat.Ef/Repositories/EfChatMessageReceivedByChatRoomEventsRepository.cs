using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bizchat.Ef.Repositories
{
    public class EfChatMessageReceivedByChatRoomEventsRepository : IChatMessageReceivedByChatRoomEventsRepository
    {

        private readonly BizchatDbContext _db;

        public EfChatMessageReceivedByChatRoomEventsRepository(BizchatDbContext db)
        {
            _db = db;
        }

        public IEnumerable<ChatMessageReceivedByChatRoomEvent> ListLatestByChatRoom(ChatRoom chatRoom, int max)
            => _db.ChatMessageReceivedByChatRoomEvents
                .Where(r => r.ChatRoom.Id == chatRoom.Id)
                .OrderByDescending(r => r.DateReceived)
                .Include(r => r.ChatMessage)
                .Include(r => r.ChatMessage.Sender)
                .Take(max);

        public void Add(ChatMessageReceivedByChatRoomEvent messageReceivedEvent)
        {
            // TODO: Dirty hack, for God knows what reason, EF tries to insert the chatroom owner >:(
            _db.Entry(messageReceivedEvent.ChatRoom).State = EntityState.Unchanged;
            _db.Entry(messageReceivedEvent.ChatMessage).State = EntityState.Unchanged;

            _db.ChatMessageReceivedByChatRoomEvents.Add(messageReceivedEvent);

            _db.SaveChanges();
        }
    }
}
