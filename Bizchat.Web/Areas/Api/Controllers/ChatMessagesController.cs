using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Repositories;
using Bizchat.Core.Verbs;
using Bizchat.Web.Areas.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bizchat.Web.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ChatMessagesController : ControllerBase
    {
        private readonly SendMessageVerb _sendMessage;
        private readonly IChatUsersRepository _chatUsersRepository;

        public ChatMessagesController(SendMessageVerb sendMessage, IChatUsersRepository chatUsersRepository)
        {
            _sendMessage = sendMessage;
            _chatUsersRepository = chatUsersRepository;
        }

        [HttpPost]
        public async Task Send(NewMessageViewModel message)
        {
            var sender = _chatUsersRepository.FindByPrincipal(User);

            await _sendMessage.Run(new ChatMessage
            {
                Contents = message.Contents,
                DateCreated = DateTime.Now,
                Destination = message.Destination,
                Sender = sender
            });
        }
    }
}