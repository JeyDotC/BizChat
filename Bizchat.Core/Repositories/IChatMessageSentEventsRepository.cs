using System;
using System.Collections.Generic;
using System.Text;
using Bizchat.Core.Events;

namespace Bizchat.Core.Repositories
{
    public interface IChatMessageSentEventsRepository
    {
        void Add(ChatMessageSentEvent chatMessageSentEvent);
    }
}
