using BloggingPlatform.Application.CommandsAndQueries.Commands.Users;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.UsersValidators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>, IRegisterUserValidator
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("Username is required.")
                    .Length(3, 60).WithMessage("Username must be between 3 and 60 characters.");

            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }

        public new async Task<ValidationResult> ValidateAsync(RegisterUserCommand dto)
        {
            return await base.ValidateAsync(dto);
        }
    }
}
