using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bizchat.Web.Areas.Api.Models
{
    public class NewMessageViewModel
    {
        public string Contents { get; set; }

        public string Destination { get; set; }
    }
}
