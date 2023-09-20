﻿using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseHA.Application.ModelDto
{
    public class AnswerCommands 
    {
        public AnswerCommands()
        {

        }
        public string Id { get; set; }

        public string IntentCodeEn { get; set; } = default!;

        public string? AnswerVn { get; set; }

        public bool Inactive { get; set; } = false;

        public bool Ondelete { get; set; }

        public virtual Category? IntentCodeEnNavigation { get; set; } 
    }
}
