using BloggingPlatform.Domain.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.CommonValidators
{
    public static class BaseMTMValidator
    {
        public static void ApplyCommonRules<T>(AbstractValidator<T> validator) where T : BaseEntityManyToMany
        {
            validator.RuleFor(x => x.CreationDate).NotEmpty().WithMessage("Creation Date is required.");
        }
    }
}
