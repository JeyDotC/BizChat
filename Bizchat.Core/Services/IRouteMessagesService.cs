using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Services
{
    public interface IRouteMessagesService
    {
        bool ICanRoute(ChatMessage message);

        bool ExcludeOtherRouters(ChatMessage message);

        MessageToQueue Route(ChatMessage message);
    }
}
