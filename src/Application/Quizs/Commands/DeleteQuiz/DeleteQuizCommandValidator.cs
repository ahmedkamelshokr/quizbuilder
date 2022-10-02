namespace Application.Quizs.Commands
{
    public class DeleteQuizCommandValidator : BaseValidator<DeleteQuizCommand>
    {
        public DeleteQuizCommandValidator(
            ) : base()
        {
            RuleFor(command => command.QuizCode)
              .NotEmpty().WithMessage("Quiz code is required!");
        }
    }
}
