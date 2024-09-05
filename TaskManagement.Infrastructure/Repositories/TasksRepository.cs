using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly ApiDbContext _context;
        public TasksRepository(ApiDbContext context)
        {
            _context = context;
        }
        public async System.Threading.Tasks.Task AddTask(Domain.Entities.Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteTask(Guid taskId, Guid userId)
        {
            var task = await GetTaskById(taskId, userId);

            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(IEnumerable<Domain.Entities.Task> Tasks, int TotalCount)> GetAllTasks(
            Statuses? status,
            DateTime? dueDate,
            Priorities? priority,
            SortByEnum sortBy,
            SortOrderEnum sortOrder,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Tasks.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(t => t.DueDate.Date == dueDate.Value.Date);
            }

            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            int totalCount = await query.CountAsync();

            switch (sortBy)
            {
                case SortByEnum.DueDate:
                    query = sortOrder == SortOrderEnum.Ascending
                        ? query.OrderBy(t => t.DueDate)
                        : query.OrderByDescending(t => t.DueDate);
                    break;

                case SortByEnum.Priority:
                    query = sortOrder == SortOrderEnum.Ascending
                        ? query.OrderBy(t => t.Priority)
                        : query.OrderByDescending(t => t.Priority);
                    break;
            }

            var tasks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tasks, totalCount);
        }

        public async Task<Domain.Entities.Task> GetTaskById(Guid taskId, Guid userId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.CreatedById == userId);
        }

        public async Task<bool> IsAnyForUser(Guid userId)
        {
            return await _context.Tasks.AnyAsync(t => t.CreatedById == userId);
        }

        public async Task<Domain.Entities.Task> UpdateTask(Guid taskId, Domain.Entities.Task task, Guid userId)
        {
            var existingTask = await GetTaskById(task.Id, userId);

            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.Status = task.Status;
                existingTask.Priority = task.Priority;
                existingTask.UpdatedAt = task.UpdatedAt;
                await _context.SaveChangesAsync();
            }

            return await GetTaskById(task.Id, userId);
        }
    }
}
