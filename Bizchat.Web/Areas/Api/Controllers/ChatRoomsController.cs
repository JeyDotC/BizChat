using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bizchat.Web.Areas.Api.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IChatRoomsRepository _chatRoomsRepository;
        private readonly IChatUsersRepository _chatUsersRepository;
        private readonly IChatMessageReceivedByChatRoomEventsRepository _receivedMessages;

        public ChatRoomsController(
            IChatRoomsRepository chatRoomsRepository, 
            IChatUsersRepository chatUsersRepository,
            IChatMessageReceivedByChatRoomEventsRepository receivedMessages)
        {
            _chatRoomsRepository = chatRoomsRepository;
            _chatUsersRepository = chatUsersRepository;
            _receivedMessages = receivedMessages;
        }

        [HttpGet]
        public IEnumerable<ChatRoom> List(
            [FromQuery] int page = 0, 
            [FromQuery] int pageSize = 10)
            => _chatRoomsRepository.List.Skip(page * pageSize).Take(pageSize);

        [HttpGet("my")]
        public IEnumerable<ChatRoom> ListUserChatRooms()
        {
            var user = _chatUsersRepository.FindByPrincipal(User);

            return _chatRoomsRepository.ListWhereUserIsMember(user);
        }

        [HttpPost("{chatRoomId}/Add/{userName}")]
        public void AddUser(int chatRoomId, string userName)
        {
            var user = _chatUsersRepository.List.First(u => u.Name == userName);

            _chatRoomsRepository.AddMember(chatRoomId, user);
        }

        [HttpPost("{chatRoomId}/Join")]
        public void Join(int chatRoomId)
        {
            var user = _chatUsersRepository.FindByPrincipal(User);

            _chatRoomsRepository.AddMember(chatRoomId, user);
        }

        [HttpGet("{chatRoomId}/Members")]
        public IEnumerable<ChatUser> Members(int chatRoomId)
            => _chatRoomsRepository.ListMembers(chatRoomId);

        [HttpGet("{chatRoomId}/Received")]
        public IEnumerable<ChatMessageReceivedByChatRoomEvent> ListReceived(int chatRoomId)
        {
            var chatRoom = _chatRoomsRepository.Find(chatRoomId);

            return _receivedMessages.ListLatestByChatRoom(chatRoom, 50).OrderBy(m => m.DateReceived);
        }

        [HttpPost("{name}")]
        public void CreateChatGroup(string name)
        {
            var user = _chatUsersRepository.FindByPrincipal(User);

            var newChatRoom = _chatRoomsRepository.Create(user, name);

            _chatRoomsRepository.AddMember(newChatRoom.Id, user);
        }
    }
}