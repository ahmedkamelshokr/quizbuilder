
namespace Application.Users.Queries
{
    public class AuthenticateUserQueryValidator : BaseValidator<AuthenticateUserQuery>
    {
        public AuthenticateUserQueryValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required!");

            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("Password is required!");
        }
    }
}
