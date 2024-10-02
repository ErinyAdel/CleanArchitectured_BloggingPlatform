using BloggingPlatform.Application.DTOs.UserDTOs;
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
        Task<ValidationResult> ValidateAsync(RegisterDTO dto);
    }
}
