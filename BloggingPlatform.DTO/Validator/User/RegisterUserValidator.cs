using BloggingPlatform.DTO.DTO.User;
using FluentValidation;

namespace BloggingPlatform.DTO.Validator.User
{
    public class RegisterUserValidator : AbstractValidator<RegisterDTO>
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
    }
}
