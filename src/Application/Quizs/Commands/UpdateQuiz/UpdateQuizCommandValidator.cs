namespace Application.Quizs.Commands
{
    public class UpdateQuizCommandValidator : BaseValidator<UpdateQuizCommand>
    {
        public UpdateQuizCommandValidator(
            ) : base()
        {
            RuleFor(command => command.QuizCode)
           .NotEmpty().WithMessage("Quiz code is required!");

            RuleFor(command => command.Title)
              .NotEmpty().WithMessage("Quiz title is required!");
        }
    }
}
