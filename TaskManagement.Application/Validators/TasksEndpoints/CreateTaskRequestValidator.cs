using FluentValidation;
using TaskManagement.Application.Contracts.TasksEndpoints;

namespace TaskManagement.Application.Validators.TasksEndpoints
{
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

            RuleFor(t => t.DueDate)
                .Must(BeFutureDate)
                .When(t => t.DueDate != default(DateTime))
                .WithMessage("Due date must be in the future.");

            RuleFor(t => t.Description)
                .Length(0, 3000)
                .When(t => !string.IsNullOrEmpty(t.Description))
                .WithMessage("Description must be less than 3000 characters.");

            RuleFor(t => t.Status)
                .IsInEnum()
                .WithMessage("Invalid Status value provided.");

            RuleFor(t => t.Priority)
                .IsInEnum()
                .WithMessage("Invalid Priority value provided.");
        }
        private bool BeFutureDate(DateTime dueDate)
        {
            return dueDate.ToUniversalTime() > DateTime.UtcNow;
        }

    }
}