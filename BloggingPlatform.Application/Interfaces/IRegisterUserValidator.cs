using BloggingPlatform.Application.CommandsAndQueries.Commands.Users;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Interfaces
{
    public interface IRegisterUserValidator
    {
        Task<ValidationResult> ValidateAsync(RegisterUserCommand dto);
    }
}
