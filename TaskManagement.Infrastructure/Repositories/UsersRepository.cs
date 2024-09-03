using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces.Repositories;

namespace TaskManagement.Infrastructure.Repositories
{

    public class UsersRepository : IUsersRepository
    {
        private readonly ApiDbContext _context;
        public UsersRepository(ApiDbContext context)
        {
            _context = context;
        }
        public async System.Threading.Tasks.Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
