using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Services;

namespace Bizchat.Core.Verbs
{
    public class SendMessageVerb : IVerb<ChatMessage>
    {
        private readonly IQueueMessagesService _messageQueuer;

        private readonly IEnumerable<IRouteMessagesService> _messageRouters;

        public SendMessageVerb(IQueueMessagesService messageQueuer, params IRouteMessagesService[] messageRouters) : this(messageQueuer, messageRouters as IEnumerable<IRouteMessagesService>)
        {
        }

        public SendMessageVerb(IQueueMessagesService messageQueuer, IEnumerable<IRouteMessagesService> messageRouters)
        {
            _messageQueuer = messageQueuer ?? throw new ArgumentNullException(nameof(messageQueuer));
            _messageRouters = messageRouters ?? throw new ArgumentNullException(nameof(messageRouters));
        }

        public async Task Run(ChatMessage message)
        {
            message.DateSent = DateTime.Now;

            var messagesToQueue = RouteMessage(message);

            foreach (var messageToQueue in messagesToQueue)
            {
               await _messageQueuer.QueueMessage(messageToQueue);
            }
        }

        private IEnumerable<ChatMessageSentEvent> RouteMessage(ChatMessage message)
        {
            var capableRouters = _messageRouters.Where(r => r.ICanRoute(message));
            var exclusiveRouter = capableRouters.FirstOrDefault(r => r.ExcludeOtherRouters(message));

            if (exclusiveRouter != null)
            {
                yield return exclusiveRouter.Route(message);
            }
            else
            {
                foreach (var router in capableRouters)
                {
                    yield return router.Route(message);
                }
            }
        }
    }
}
