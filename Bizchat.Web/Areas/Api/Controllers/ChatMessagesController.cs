using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Verbs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bizchat.Web.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly SendMessageVerb _sendMessage;

        public ChatMessagesController(SendMessageVerb sendMessage)
        {
            _sendMessage = sendMessage;
        }

        [HttpPost]
        async Task Send(ChatMessage message)
            => await _sendMessage.Run(message);
    }
}