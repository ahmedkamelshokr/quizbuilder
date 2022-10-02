namespace Application.Quizs.Commands
{
    public class CreateQuizCommandValidator : BaseValidator<CreateQuizCommand>
    {
        public CreateQuizCommandValidator(
            ) : base()
        {
            RuleFor(command => command.Title)
              .NotEmpty().WithMessage("Quiz title is required!");

            RuleFor(command => command.Questions)
           .Must(a => a.Count() <= 10).WithMessage("Quiz can not have more then 10 questions!");

            RuleForEach(command => command.Questions)
           .SetValidator(new QuestionValidator());

        }
    }

    public class QuestionValidator : BaseValidator<CQuestionDto>
    {
        public QuestionValidator(
            ) : base()
        {
            RuleFor(q => q.Description)
              .NotEmpty().WithMessage("Question description is required!");

            RuleFor(q => q.Answers)
         .Must(a => a.Count() <= 5).WithMessage("Question can not have more then 5 answers!");

            RuleFor(q => q.Questiontype)
         .NotNull().WithMessage("Question type is required!")
         .IsInEnum().WithMessage("Question type is not valid!");

            RuleForEach(command => command.Answers)
           .SetValidator(new AnswerValidator());
        }
    }

    public class AnswerValidator : BaseValidator<CAnswerDto>
    {
        public AnswerValidator(
            ) : base()
        {
            RuleFor(q => q.Description)
              .NotEmpty().WithMessage("Question description is required!");

            RuleFor(q => q.IsCorrect)
         .NotNull().WithMessage("Answer IsCorrect is required!");
        }
    }
}
