namespace Application.Quizs.Commands
{
    public class SubmitQuizCommandValidator : BaseValidator<SubmitQuizCommand>
    {
        public SubmitQuizCommandValidator(
            ) : base()
        {
            RuleFor(command => command.QuizQuestions)
              .NotNull().WithMessage("Quiz Answers is required!");

            RuleFor(command => command.QuizCode)
              .NotEmpty().WithMessage("Quiz code is required!");
        }
    }
}
