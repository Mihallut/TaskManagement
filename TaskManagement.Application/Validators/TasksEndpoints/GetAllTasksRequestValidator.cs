using FluentValidation;
using TaskManagement.Application.Contracts.TasksEndpoints;

namespace TaskManagement.Application.Validators.TasksEndpoints
{
    public class GetAllTasksRequestValidator : AbstractValidator<GetAllTasksRequest>
    {
        public GetAllTasksRequestValidator()
        {
            RuleFor(t => t.SortBy)
                .IsInEnum()
                .WithMessage("Invalid SortBy value provided.");

            RuleFor(t => t.SortOrder)
                .IsInEnum()
                .WithMessage("Invalid SortOrder value provided.");

            RuleFor(t => t.Status)
                .IsInEnum()
                .WithMessage("Invalid Status value provided.")
                .When(t => t.Status != null);

            RuleFor(t => t.Priority)
                .IsInEnum()
                .WithMessage("Invalid Priority value provided.")
                .When(t => t.Status != null);
        }
    }
}
