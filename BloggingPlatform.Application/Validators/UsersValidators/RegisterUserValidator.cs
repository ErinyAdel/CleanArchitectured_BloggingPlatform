using BloggingPlatform.Application.DTOs.UserDTOs;
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
    public class RegisterUserValidator : BaseUserValidator<RegisterDTO>, IRegisterUserValidator
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("Username is required.")
                    .Length(3, 60).WithMessage("Username must be between 3 and 60 characters.");

            //RuleFor(x => x.PhoneNumber).Matches(@"^\+?\d{10,15}$").WithMessage("Invalid Phone Number.");
        }

        public new async Task<ValidationResult> ValidateAsync(RegisterDTO dto)
        {
            return await base.ValidateAsync(dto);
        }
    }
}
