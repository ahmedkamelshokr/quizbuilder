namespace Application.Quizs.Queries
{

    public class GetExaminerQuizResultValidator : BaseValidator<GetQuizForEditQuery>
    {

        public GetExaminerQuizResultValidator() : base()
        {

            RuleFor(query => query.QuizCode)
                .NotEmpty().WithMessage("Quiz code Id is required.");

        }
    }
}
