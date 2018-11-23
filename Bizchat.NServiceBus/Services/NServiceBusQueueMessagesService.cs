using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Events;
using Bizchat.Core.Services;
using NServiceBus;

namespace Bizchat.NServiceBus.Services
{
    public class NServiceBusQueueMessagesService : IQueueMessagesService
    {
        private readonly IMessageSession _endpoint;

        public NServiceBusQueueMessagesService(IMessageSession endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task QueueMessage(ChatMessageSentEvent message)
        {
            await _endpoint.Publish(message)
                .ConfigureAwait(false);
        }
    }
}
