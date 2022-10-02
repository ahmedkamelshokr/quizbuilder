namespace Application.Quizs.Queries
{

    public class GetQuizQueryValidator : BaseValidator<GetQuizForExaminerQuery>
    {

        public GetQuizQueryValidator() : base()
        {

            RuleFor(query => query.QuizCode)
                .NotEmpty().WithMessage("Quiz code Id is required.");

        }
    }
}
