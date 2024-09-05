using TaskManagement.Application.Mappings;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.ViewModels
{
    public class TaskShortInfo : IMapFrom<Domain.Entities.Task>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public Statuses Status { get; set; }
        public Priorities Priority { get; set; }
    }
}