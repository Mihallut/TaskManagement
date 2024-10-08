﻿using FluentValidation;
using TaskManagement.Application.Contracts.UsersEndpoints;
using TaskManagement.Application.Interfaces.Repositories;

namespace TaskManagement.Application.Validators.UsersEndpoints
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        private readonly IUsersRepository _usersRepository;

        public LoginUserRequestValidator(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(BeRegistredEmail).WithMessage("There are no registered users for this email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }

        private bool BeRegistredEmail(string email)
        {
            return _usersRepository.UserExistsByEmail(email);
        }
    }
}
