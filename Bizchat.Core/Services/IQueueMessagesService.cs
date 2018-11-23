using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Events;

namespace Bizchat.Core.Services
{
    public interface IQueueMessagesService
    {
        Task QueueMessage(ChatMessageSentEvent message);
    }
}
