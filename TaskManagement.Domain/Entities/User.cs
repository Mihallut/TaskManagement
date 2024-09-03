using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Domain.Entities.Task> Tasks { get; set; }
    }
}
