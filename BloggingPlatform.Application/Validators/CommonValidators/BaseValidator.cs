using BloggingPlatform.Domain.Common;
using BloggingPlatform.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.CommonValidators
{
    public static class BaseValidator
    {
        public static void ApplyCommonRules<T>(AbstractValidator<T> validator) where T : BaseEntity
        {
            validator.RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            validator.RuleFor(x => x.CreationDate).NotEmpty().WithMessage("Creation Date is required.");
        }
    }
}
