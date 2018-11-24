using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;

namespace Bizchat.Core.Repositories
{
    public interface IChatMessageReceivedByChatRoomEventsRepository
    {
        IEnumerable<ChatMessageReceivedByChatRoomEvent> ListLatestByChatRoom(ChatRoom chatRoom, int max);

        void Add(ChatMessageReceivedByChatRoomEvent messageReceivedEvent);
    }
}
