using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bizchat.Core.Services
{
    public interface IQueueMessagesService
    {
        Task QueueMessage(MessageToQueue message);
    }
}
