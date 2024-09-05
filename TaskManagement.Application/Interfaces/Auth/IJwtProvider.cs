using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
