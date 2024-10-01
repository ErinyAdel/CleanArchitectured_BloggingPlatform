using BloggingPlatform.Application.DTOs.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.UsersValidators
{
    public class UserLoginValidator : BaseUserValidator<LoginDTO>
    {
    }
}
