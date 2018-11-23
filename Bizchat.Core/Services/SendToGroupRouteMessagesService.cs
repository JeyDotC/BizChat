using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;
using Bizchat.Core.Repositories;
using System.Linq;
using Bizchat.Core.Events;

namespace Bizchat.Core.Services
{
    public class SendToGroupRouteMessagesService : IRouteMessagesService
    {
        private readonly IChatRoomsRepository _chatRoomsRepository;
        private readonly IChatMessagesRepository _messagesRepository;

        public SendToGroupRouteMessagesService(IChatRoomsRepository chatRoomsRepository, IChatMessagesRepository messagesRepository)
        {
            _chatRoomsRepository = chatRoomsRepository;
            _messagesRepository = messagesRepository;
        }

        public bool ExcludeOtherRouters(ChatMessage message) => false;

        public bool ICanRoute(ChatMessage message) => 
                int.TryParse(message.Destination, out var roomId) &&
                _chatRoomsRepository.Find(roomId) != null;

        public ChatMessageSentEvent Route(ChatMessage message)
        {
            _messagesRepository.Add(message);

            return new ChatMessageSentEvent
            {
                ExtraInfo = int.Parse(message.Destination),
                Contents = message,
                RoutingKey = "Groups"
            };
        }
    }
}
