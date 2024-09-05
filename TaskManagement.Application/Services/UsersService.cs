using TaskManagement.Application.Interfaces.Auth;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;

        public UsersService(IPasswordHasher passwordHasher, IUsersRepository usersRepository, IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
        }
        public async System.Threading.Tasks.Task Register(string username, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var userEntity = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            await _usersRepository.Add(userEntity);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmail(email);
            if (user != null)
            {
                var result = _passwordHasher.Verify(password, user.PasswordHash);
                if (result)
                {
                    var token = _jwtProvider.GenerateToken(user);
                    return token;
                }
            }

            return null;
        }
    }
}
