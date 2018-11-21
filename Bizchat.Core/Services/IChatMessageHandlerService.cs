using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Services
{
    public interface IChatMessageHandlerService
    {
        void ReceiveMessage(ChatMessage message);
    }
}
