using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizchat.Web.Controllers
{
    [Authorize]
    public class ChatRoomsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}