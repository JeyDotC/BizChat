using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizchat.Core.Events;
using Bizchat.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using NServiceBus;

namespace Bizchat.Web.BusHandlers
{
    // SignalR symply doesn't work!
    public class ChatMessageReceivedByChatRoomHandler : IHandleMessages<ChatMessageReceivedByChatRoomEvent>
    {
        private readonly HubConnection _hub;

        public ChatMessageReceivedByChatRoomHandler(HubConnection hub)
        {
            _hub = hub;
        }
        public async Task Handle(ChatMessageReceivedByChatRoomEvent message, IMessageHandlerContext context)
        {
            await _hub.InvokeAsync("MessageReceivedByChatRoom", message);
        }
    }
}
