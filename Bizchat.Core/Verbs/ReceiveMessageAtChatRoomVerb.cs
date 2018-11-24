using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Exceptions;
using Bizchat.Core.Repositories;

namespace Bizchat.Core.Verbs
{
    public class ReceiveMessageAtChatRoomVerb : IVerb<ChatMessageSentEvent, ChatMessageReceivedByChatRoomEvent>
    {
        private static readonly Regex _chatRoomProtocol = new Regex(@"^chatroom://(\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly IChatRoomsRepository _chatRooms;
        private readonly IChatMessageReceivedByChatRoomEventsRepository _chatMessageReceivedEvents;
        private readonly IChatMessagesRepository _messages;

        public ReceiveMessageAtChatRoomVerb(IChatRoomsRepository chatRooms, IChatMessageReceivedByChatRoomEventsRepository chatMessageReceivedEvents, IChatMessagesRepository messages)
        {
            _chatRooms = chatRooms;
            _chatMessageReceivedEvents = chatMessageReceivedEvents;
            _messages = messages;
        }

        public async Task<ChatMessageReceivedByChatRoomEvent> Run(ChatMessageSentEvent message)
        {
            var chatMessage = _messages.Find(message.Contents.Id);
            var chatRoomId = CalculateChatRoomId(chatMessage.Destination);
            var chatRoom = _chatRooms.Find(chatRoomId);

            if (!_chatRooms.ListMembers(chatRoom.Id).Any(m => chatMessage.Sender.Id == m.Id))
            {
                throw new PermissionException("User is not a member of this chat room.");
            }

            var messageReceivedEvent = new ChatMessageReceivedByChatRoomEvent
            {
                ChatMessage = chatMessage,
                ChatRoom = chatRoom,
                DateReceived = DateTime.Now
            };

            _chatMessageReceivedEvents.Add(messageReceivedEvent);

            return await Task.FromResult(messageReceivedEvent);
        }

        private static int CalculateChatRoomId(string destination)
        {
            var match = _chatRoomProtocol.Match(destination);

            if (!match.Success)
            {
                throw new InvalidDestinationException("Destination syntax is incorrect.");
            }

            return int.Parse(match.Groups[1].Value);
        }
    }
}
