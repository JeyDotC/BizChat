using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;

namespace Bizchat.Core.Services
{
    public interface IRouteMessagesService
    {
        bool ICanRoute(ChatMessage message);

        bool ExcludeOtherRouters(ChatMessage message);

        ChatMessageSentEvent Route(ChatMessage message);
    }
}
