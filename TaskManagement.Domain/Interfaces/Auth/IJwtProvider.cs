using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
