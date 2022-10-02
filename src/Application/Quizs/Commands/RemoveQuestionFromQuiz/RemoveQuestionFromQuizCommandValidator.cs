namespace Application.Quizs.Commands
{
    public class RemoveQuestionFromQuizCommandValidator : BaseValidator<RemoveQuestionFromQuizCommand>
    {
        public RemoveQuestionFromQuizCommandValidator(
            ) : base()
        {
            RuleFor(command => command.QuizCode)
              .NotEmpty().WithMessage("Quiz code is required!");
             

            RuleFor(command => command.QuestionsIds)
             .NotNull().WithMessage("Question id(s) is required!");
        }
    }
}
