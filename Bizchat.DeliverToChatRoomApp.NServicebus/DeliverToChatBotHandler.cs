using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Events;
using Bizchat.StockChatBot;
using NServiceBus;

namespace Bizchat.DeliverToChatRoomApp.NServicebus
{
    public class DeliverToChatBotHandler : IHandleMessages<ChatMessageSentEvent>
    {
        private readonly SearchStockVerb _searchStockVerb;

        public DeliverToChatBotHandler(SearchStockVerb searchStockVerb)
        {
            _searchStockVerb = searchStockVerb;
        }

        public async Task Handle(ChatMessageSentEvent message, IMessageHandlerContext context)
        {
            await _searchStockVerb.Run(message);
        }
    }
}
