using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Repositories;

namespace Bizchat.Web
{
    internal static class IChatUsersRepositoryExtensions
    {

        public static ChatUser FindByPrincipal(this IChatUsersRepository self, ClaimsPrincipal principal)
        {
            var webUserId = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return self.List.First(u => u.UserId == webUserId);
        }
    }
}
