using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Repositories;

namespace Bizchat.Core.Verbs
{
    public class ReceiveMessageVerb : IVerb<ChatMessageSentEvent>
    {
        private readonly IChatRoomsRepository _chatRooms;
        private readonly IChatMessagesRepository _chatMessages;

        public ReceiveMessageVerb(IChatRoomsRepository chatRooms, IChatMessagesRepository chatMessages)
        {
            _chatRooms = chatRooms;
            _chatMessages = chatMessages;
        }

        public async Task Run(ChatMessageSentEvent message)
        {
            if (message.RoutingKey != "Groups")
            {
                return;
            }

            var chatMessage = message.Contents;


            _chatRooms.ReceiveMessage((int)message.ExtraInfo, chatMessage);

            chatMessage.DateReceived = DateTime.Now;

            _chatMessages.Update(chatMessage);

            await Task.CompletedTask;
        }
    }
}
