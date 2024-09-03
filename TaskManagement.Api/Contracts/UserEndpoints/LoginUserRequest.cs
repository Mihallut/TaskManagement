namespace TaskManagement.Api.Contracts.UserEndpoints
{
    public record LoginUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
