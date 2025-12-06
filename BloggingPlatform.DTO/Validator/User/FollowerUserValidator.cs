using BloggingPlatform.Application.Validators.CommonValidators;
using BloggingPlatform.Domain.Entities;
using FluentValidation;

namespace BloggingPlatform.DTO.Validator.User
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
