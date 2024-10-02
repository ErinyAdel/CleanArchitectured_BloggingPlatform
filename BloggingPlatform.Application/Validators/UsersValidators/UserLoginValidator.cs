using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.UsersValidators
{
    public class UserLoginValidator : BaseUserValidator<LoginDTO>, IUserLoginValidator
    {
        public new async Task<ValidationResult> ValidateAsync(LoginDTO dto)
        {
            return await base.ValidateAsync(dto);
        }
    }
}
