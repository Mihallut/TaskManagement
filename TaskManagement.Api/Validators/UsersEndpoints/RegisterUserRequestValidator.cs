using FluentValidation;
using TaskManagement.Api.Contracts.UsersEndpoints;
using TaskManagement.Domain.Interfaces.Repositories;

namespace TaskManagement.Api.Validators.UsersEndpoints
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        private readonly IUsersRepository _usersRepository;

        public RegisterUserRequestValidator(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
                .Matches("^[a-zA-Z0-9]+$").WithMessage("Username can only contain letters and numbers, without spaces.")
                .Must(BeUniqueUsername).WithMessage("This username was already registered.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(BeUniqueEmail).WithMessage("This email was already registered.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"^(?=.*[!""#$%&'()*+,-./:;<=>?@[\\]^_`{|}~])[A-Za-z0-9!""#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]+$")
                .WithMessage("Your password must contain Latin letters and digits and at least one special symbol");
        }

        private bool BeUniqueUsername(string username)
        {
            return !_usersRepository.UserExistsByUsername(username);
        }

        private bool BeUniqueEmail(string email)
        {
            return !_usersRepository.UserExistsByEmail(email);
        }
    }
}
