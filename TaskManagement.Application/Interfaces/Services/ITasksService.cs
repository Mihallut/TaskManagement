using TaskManagement.Application.Contracts.TasksEndpoints;
using TaskManagement.Application.ViewModels;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITasksService
    {
        Task CreateTask(CreateTaskRequest request, Guid createdBy);
        Task<PagedResult<TaskShortInfo>> GetTasks(GetAllTasksRequest request, Guid userId);
        Task<TaskDto> GetTaskById(Guid taskId, Guid userId);
        Task<TaskDto> UpdateTask(Guid id, UpdateTaskRequest request, Guid userId);
        Task DeleteTask(Guid taskId, Guid userId);
    }
}
