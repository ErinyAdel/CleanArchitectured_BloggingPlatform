using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.UsersValidators
{
    public class UserValidator : AbstractValidator<RegisterDTO>
    {
        public UserValidator()
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

            //RuleFor(x => x.PhoneNumber).Matches(@"^\+?\d{10,15}$").WithMessage("Invalid Phone Number.");
        }
    }
}
