using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Contracts.TasksEndpoints
{
    public record GetAllTasksRequest
    {
        public Statuses? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public Priorities? Priority { get; set; }

        public SortByEnum SortBy { get; set; } = SortByEnum.DueDate;
        public SortOrderEnum SortOrder { get; set; } = SortOrderEnum.Ascending;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
