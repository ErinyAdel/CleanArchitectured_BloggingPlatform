using BloggingPlatform.Application.Validators.CommonValidators;
using BloggingPlatform.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.PostsValidators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            BaseValidator.ApplyCommonRules(this);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Post Title is required.")
                .Length(1, 50).WithMessage("Post Title must be between 1 and 50 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post Content is required.")
                .Length(1, 256).WithMessage("Name must be between 1 and 256 characters.");
        }
    }
}
