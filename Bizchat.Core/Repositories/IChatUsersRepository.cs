using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Repositories
{
    public interface IChatUsersRepository
    {
        void Add(ChatUser chatUser);

        IQueryable<ChatUser> List { get; }
    }
}
