using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Services
{
    public class MessageSenderService
    {
        private readonly IQueueMessagesService _messageQueuer;

        private readonly IEnumerable<IRouteMessagesService> _messageRouters;

        public MessageSenderService(IQueueMessagesService messageQueuer, params IRouteMessagesService[] messageRouters) : this(messageQueuer, messageRouters as IEnumerable<IRouteMessagesService>)
        {
        }

        public MessageSenderService(IQueueMessagesService messageQueuer, IEnumerable<IRouteMessagesService> messageRouters)
        {
            _messageQueuer = messageQueuer ?? throw new ArgumentNullException(nameof(messageQueuer));
            _messageRouters = messageRouters ?? throw new ArgumentNullException(nameof(messageRouters));
        }

        public async Task SendMessage(ChatMessage message)
        {
            var messagesToQueue = RouteMessage(message);

            foreach (var messageToQueue in messagesToQueue)
            {
               await _messageQueuer.QueueMessage(messageToQueue);
            }
        }

        private IEnumerable<MessageToQueue> RouteMessage(ChatMessage message)
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
