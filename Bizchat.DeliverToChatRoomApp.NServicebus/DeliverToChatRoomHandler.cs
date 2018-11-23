using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Events;
using Bizchat.Core.Repositories;
using Bizchat.Core.Services;
using Bizchat.Core.Verbs;
using NServiceBus;

namespace Bizchat.DeliverToChatRoomApp.NServicebus
{
    public class DeliverToChatRoomHandler : IHandleMessages<ChatMessageSentEvent>
    {
        private readonly ReceiveMessageVerb _receiveMessageVerb;

        public DeliverToChatRoomHandler(ReceiveMessageVerb receiveMessageVerb)
        {
            _receiveMessageVerb = receiveMessageVerb;
        }

        public async Task Handle(ChatMessageSentEvent message, IMessageHandlerContext context)
        {
            await _receiveMessageVerb.Run(message);

            await context.Publish(new ChatMessageReceivedEvent
            {
                ChatMessage = message.Contents
            });
        }
    }
}
