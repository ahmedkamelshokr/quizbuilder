namespace Application.Quizs.Queries
{

    public class GetQuizResultStatsQueryValidator : BaseValidator<GetQuizResultStatsQuery>
    {

        public GetQuizResultStatsQueryValidator() : base()
        {

            RuleFor(query => query.QuizCode)
                .NotEmpty().WithMessage("Quiz code Id is required.");

        }
    }
}
