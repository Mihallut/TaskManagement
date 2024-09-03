using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        public System.Threading.Tasks.Task Add(User user);
        public Task<User> GetByEmail(string email);
        public bool UserExistsByUsername(string username);
        public bool UserExistsByEmail(string email);
    }
}
