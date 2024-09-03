﻿namespace TaskManagement.Api.Contracts.UserEndpoints
{
    public record RegisterUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
