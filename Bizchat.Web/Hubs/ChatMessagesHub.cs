using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizchat.Core.Events;
using Microsoft.AspNetCore.SignalR;

namespace Bizchat.Web.Hubs
{
    public class ChatMessagesHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task MessageReceivedByChatRoom(ChatMessageReceivedByChatRoomEvent messageReceived)
             => await Clients.All.SendAsync(nameof(MessageReceivedByChatRoom), messageReceived);
    }
}
