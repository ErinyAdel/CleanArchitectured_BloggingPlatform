using BloggingPlatform.Application.Validators.CommonValidators;
using BloggingPlatform.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Validators.UsersValidators
{
    public class FollowerUserValidator : AbstractValidator<Follower>
    {
        public FollowerUserValidator()
        {
            BaseMTMValidator.ApplyCommonRules(this);

            RuleFor(x => new { x.FollowerUserId, x.FollowedUserId }).NotEmpty().WithMessage("User Id is required.");
            RuleFor(x => x).Must(f => f.FollowerUserId != f.FollowedUserId).WithMessage("A user cannot follow themselves.");
        }
    }
}
