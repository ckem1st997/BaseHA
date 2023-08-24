using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class Chat: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string? ConversationId { get; set; }


        public string? MsgId { get; set; }


        public int? MessageIndex { get; set; }


        public string? Content { get; set; }


        public string? Time { get; set; }


        public int? ServiceId { get; set; }


        public string? StartTime { get; set; }


        public string? SenderAgentName { get; set; }


        public int? SenderAgentId { get; set; }


        public string? SenderVisitorName { get; set; }


        public int? SenderVisitorId { get; set; }


        public int? LastAgentUserId { get; set; }


        public int? TicketId { get; set; }


        public int? Type { get; set; }


        public int? ConversationType { get; set; }


        public int? RequesterId { get; set; }


        public override bool OnDelete { get; set; } = default!;

    }
}
