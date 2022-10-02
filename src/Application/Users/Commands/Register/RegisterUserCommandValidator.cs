namespace Application.Users.Commands
{
    public class RegisterUserCommandValidator : BaseValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator() : base()
        {
            RuleFor(command => command.FirstName)
                .NotEmpty().WithMessage("First Name is required!");

            RuleFor(command => command.LastName)
                .NotEmpty().WithMessage("Last Name is required!");

            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("Password is required!");
            RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Email is required!");
        }

    }
}

