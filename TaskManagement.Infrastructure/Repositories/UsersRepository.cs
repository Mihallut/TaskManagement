﻿using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;

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

        public bool UserExistsByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool UserExistsByUsername(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }
    }
}
