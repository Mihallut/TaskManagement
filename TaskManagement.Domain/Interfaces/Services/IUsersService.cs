
namespace TaskManagement.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        Task Register(string username, string email, string password);

        Task<string> Login(string email, string password);
    }
}