namespace Application.Quizs.Queries
{

    public class GetExaminerQuizResultQueryValidator : BaseValidator<GetExaminerQuizResultQuery>
    {

        public GetExaminerQuizResultQueryValidator() : base()
        {

            RuleFor(query => query.QuizCode)
                .NotEmpty().WithMessage("Quiz code Id is required.");

        }
    }
}
