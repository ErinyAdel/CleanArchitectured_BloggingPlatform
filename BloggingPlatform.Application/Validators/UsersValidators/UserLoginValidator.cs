using BloggingPlatform.Application.CommandsAndQueries.Commands.Users;
using BloggingPlatform.Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace BloggingPlatform.Application.Validators.UsersValidators
{
    public class UserLoginValidator : AbstractValidator<UserLoginCommand>, IUserLoginValidator
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
        public new async Task<ValidationResult> ValidateAsync(UserLoginCommand dto)
        {
            return await base.ValidateAsync(dto);
        }
    }
}
