using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Repositories;
using Bizchat.Core.Services;

namespace Bizchat.Core.Verbs
{
    public class SendMessageVerb : IVerb<ChatMessage, ChatMessageSentEvent>
    {
        private readonly IQueueMessagesService _messageQueuer;
        private readonly IChatMessagesRepository _chatMessagesRepository;
        private readonly IChatMessageSentEventsRepository _messageSentEvents;

        public SendMessageVerb(
            IQueueMessagesService messageQueuer, 
            IChatMessagesRepository chatMessagesRepository,
            IChatMessageSentEventsRepository messageSentEvents)
        {
            _messageQueuer = messageQueuer;
            _chatMessagesRepository = chatMessagesRepository;
            _messageSentEvents = messageSentEvents;
        }

        public async Task<ChatMessageSentEvent> Run(ChatMessage message)
        {
            message.DateCreated = DateTime.Now;

            _chatMessagesRepository.Add(message);

            var chatMessageSentEvent = new ChatMessageSentEvent
            {
                Contents = message,
                DateSent = DateTime.Now
            };

            _messageSentEvents.Add(chatMessageSentEvent);

            await _messageQueuer.QueueMessage(chatMessageSentEvent);
            
            return chatMessageSentEvent;
        }
    }
}
