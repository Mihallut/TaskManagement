using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Contracts.TasksEndpoints
{
    public record UpdateTaskRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Statuses Status { get; set; }
        public Priorities Priority { get; set; }
    }
}
