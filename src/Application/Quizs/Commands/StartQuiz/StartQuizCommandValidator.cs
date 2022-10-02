namespace Application.Quizs.Commands
{
    public class StartQuizCommandValidator : BaseValidator<StartQuizCommand>
    {
        public StartQuizCommandValidator(
            ) : base()
        {
            RuleFor(command => command.QuizCode)
              .NotEmpty().WithMessage("Quiz code is required!");


        }
    }
}
