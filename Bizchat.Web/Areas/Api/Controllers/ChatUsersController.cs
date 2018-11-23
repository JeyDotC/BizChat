using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bizchat.Web.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatUsersController : ControllerBase
    {
        private readonly IChatUsersRepository _chatUsersRepository;

        public ChatUsersController(IChatUsersRepository chatUsersRepository)
        {
            _chatUsersRepository = chatUsersRepository;
        }

        [HttpGet("Search")]
        public IEnumerable<ChatUser> Search([FromQuery] string q)
            => _chatUsersRepository.List.Where(u => u.Name.Contains(q));

        [HttpGet]
        public IEnumerable<ChatUser> List()
            => _chatUsersRepository.List.AsEnumerable();
    }
}