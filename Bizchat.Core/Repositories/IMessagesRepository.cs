﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bizchat.Core.Entities;

namespace Bizchat.Core.Repositories
{
    public interface IMessagesRepository
    {
        IQueryable<ChatMessage> List { get; }

        void Add(ChatMessage message);
    }
}
