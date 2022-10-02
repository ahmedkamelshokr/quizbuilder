namespace Application.Quizs.Commands
{
    public class AddQuestionToQuizCommandValidator : BaseValidator<AddQuestionToQuizCommand>
    {
        public AddQuestionToQuizCommandValidator(
            ) : base()
        {
            RuleFor(command => command.QuizCode)
              .NotEmpty().WithMessage("Quiz code is required!");


            RuleFor(command => command.Questions)
             .NotNull().WithMessage("Question is required!");


        }
    }
}
