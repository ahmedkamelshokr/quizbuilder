namespace Application.Quizs.Commands
{
    public class PublishQuizCommandValidator : BaseValidator<PublishQuizCommand>
    {
        public PublishQuizCommandValidator(
            ) : base()
        {
            RuleFor(command => command.QuizCode)
              .NotEmpty().WithMessage("Quiz code is required!");
             //.MustAsync(IsExist).WithMessage("Quiz code is not exist");
        }
    }
}
