﻿namespace BaseHA.Application.ModelDto
{
    public class BaseCommands
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
