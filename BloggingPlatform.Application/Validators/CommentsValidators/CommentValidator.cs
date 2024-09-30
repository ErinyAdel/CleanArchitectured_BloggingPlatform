using BloggingPlatform.Application.Validators.CommonValidators;
using BloggingPlatform.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.CommentsValidators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            BaseValidator.ApplyCommonRules(this);

            RuleFor(x => x.CommentText)
                .NotEmpty().WithMessage("Comment is required.")
                .Length(1, 100).WithMessage("Comment must be between 1 and 100 characters.");
        }
    }
}
