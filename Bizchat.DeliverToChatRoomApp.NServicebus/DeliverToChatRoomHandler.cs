using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Events;
using Bizchat.Core.Exceptions;
using Bizchat.Core.Repositories;
using Bizchat.Core.Services;
using Bizchat.Core.Verbs;
using NServiceBus;

namespace Bizchat.DeliverToChatRoomApp.NServicebus
{
    public class DeliverToChatRoomHandler : IHandleMessages<ChatMessageSentEvent>
    {
        private readonly ReceiveMessageAtChatRoomVerb _receiveMessageVerb;
        private readonly IChatRoomsRepository _chatRoomsRepository;

        public DeliverToChatRoomHandler(ReceiveMessageAtChatRoomVerb receiveMessageVerb, IChatRoomsRepository chatRoomsRepository)
        {
            _receiveMessageVerb = receiveMessageVerb;
            _chatRoomsRepository = chatRoomsRepository;
        }

        public async Task Handle(ChatMessageSentEvent message, IMessageHandlerContext context)
        {

            try
            {
                var result = await _receiveMessageVerb.Run(message);
                await context.Publish(result);
            }
            catch (InvalidDestinationException)
            {
                // Assume the message is not for me, ignore for now. The exception should also be logged.
                return;
            }
            catch(PermissionException ex)
            {
                await context.Publish(new ChatMessageRejectedEvent
                {
                    DateRejected = DateTime.Now,
                    OriginalMessage = message,
                    Reason = ex.Message
                });
            }
        }
    }
}
