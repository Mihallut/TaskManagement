using AutoMapper;
using TaskManagement.Application.Contracts.TasksEndpoints;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.ViewModels;

namespace TaskManagement.Application.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _repository;
        private readonly IMapper _mapper;
        public TasksService(ITasksRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async System.Threading.Tasks.Task CreateTask(CreateTaskRequest request, Guid createdBy)
        {
            var task = new Domain.Entities.Task
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate.ToUniversalTime(),
                Status = request.Status,
                Priority = request.Priority,
                CreatedAt = DateTime.UtcNow,
                CreatedById = createdBy
            };

            await _repository.AddTask(task);
        }

        public async System.Threading.Tasks.Task DeleteTask(Guid taskId, Guid userId)
        {
            await _repository.DeleteTask(taskId, userId);
        }

        public async Task<TaskDto> GetTaskById(Guid taskId, Guid userId)
        {
            var task = await _repository.GetTaskById(taskId, userId);
            if (task == null)
            {
                return null;
            }
            var taskDto = _mapper.Map<TaskDto>(task);
            return taskDto;
        }

        public async Task<PagedResult<TaskShortInfo>> GetTasks(GetAllTasksRequest request, Guid userId)
        {
            if (!await _repository.IsAnyForUser(userId))
            {
                return null;
            }

            var (tasks, totalItems) = await _repository.GetAllTasks(
                request.Status,
                request.DueDate,
                request.Priority,
                request.SortBy,
                request.SortOrder,
                request.PageNumber,
                request.PageSize);

            var tasksDto = _mapper.Map<List<TaskShortInfo>>(tasks);

            var pagedResult = new PagedResult<TaskShortInfo>
            {
                Items = tasksDto,
                TotalItems = totalItems,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return pagedResult;
        }

        public async Task<TaskDto> UpdateTask(Guid id, UpdateTaskRequest request, Guid userId)
        {
            var taskToUpdate = new Domain.Entities.Task
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
                Status = request.Status,
                UpdatedAt = DateTime.UtcNow
            };

            var updatedTask = await _repository.UpdateTask(id, taskToUpdate, userId);
            var updatedTaskDto = _mapper.Map<TaskDto>(updatedTask);
            return updatedTaskDto;
        }
    }
}
