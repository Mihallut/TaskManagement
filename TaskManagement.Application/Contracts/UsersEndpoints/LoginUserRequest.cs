namespace TaskManagement.Application.Contracts.UsersEndpoints
{
    public record LoginUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
