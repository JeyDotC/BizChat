﻿using System;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Ef.Tables;
using Microsoft.EntityFrameworkCore;

namespace Bizchat.Ef
{
    public class BizchatDbContext : DbContext
    {
        public BizchatDbContext(DbContextOptions<BizchatDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }

        public DbSet<ChatRoomMembership> ChatRoomMemberships { get; set; }

        public DbSet<ChatMessageSentEvent> ChatMessageSentEvents { get; set; }

        public DbSet<ChatMessageReceivedByChatRoomEvent> ChatMessageReceivedByChatRoomEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatRoomMembership>().HasKey(m => new { m.ChatRoomId, m.ChatUserId });
        }
    }
}
