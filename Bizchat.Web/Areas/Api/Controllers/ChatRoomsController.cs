﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
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

        private string WebUserId => User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

        private ChatUser GetCurrentChatUser() => _chatUsersRepository.List.First(u => u.UserId == WebUserId);

        public ChatRoomsController(IChatRoomsRepository chatRoomsRepository, IChatUsersRepository chatUsersRepository)
        {
            _chatRoomsRepository = chatRoomsRepository;
            _chatUsersRepository = chatUsersRepository;
        }

        [HttpGet]
        public IEnumerable<ChatRoom> List(
            [FromQuery] int page = 0, 
            [FromQuery] int pageSize = 10)
            => _chatRoomsRepository.List.Skip(page * pageSize).Take(pageSize);

        [HttpGet("my")]
        public IEnumerable<ChatRoom> ListUserChatRooms()
        {
            var user = GetCurrentChatUser();

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
            var user = GetCurrentChatUser();

            _chatRoomsRepository.AddMember(chatRoomId, user);
        }

        [HttpGet("{chatRoomId}/Members")]
        public IEnumerable<ChatUser> Members(int chatRoomId)
            => _chatRoomsRepository.ListMembers(chatRoomId);

        [HttpPost("{name}")]
        public void CreateChatGroup(string name)
        {
            var user = GetCurrentChatUser();

            var newChatRoom = _chatRoomsRepository.Create(user, name);

            _chatRoomsRepository.AddMember(newChatRoom.Id, user);
        }
    }
}