using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface ITasksRepository
    {
        Task AddTask(Domain.Entities.Task task);
        Task<Domain.Entities.Task> GetTaskById(Guid taskId, Guid userId);
        Task DeleteTask(Guid taskId, Guid userId);
        Task<Domain.Entities.Task> UpdateTask(Guid taskId, Domain.Entities.Task task, Guid userId);
        Task<(IEnumerable<Domain.Entities.Task> Tasks, int TotalCount)> GetAllTasks(
            Statuses? status,
            DateTime? dueDate,
            Priorities? priority,
            SortByEnum sortBy,
            SortOrderEnum sortOrder,
            int pageNumber,
            int pageSize);
        Task<bool> IsAnyForUser(Guid userId);
    }
}
