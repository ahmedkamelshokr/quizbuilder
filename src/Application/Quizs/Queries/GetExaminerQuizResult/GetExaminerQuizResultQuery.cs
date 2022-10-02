namespace Application.Quizs.Queries
{
    public class GetExaminerQuizResultQuery : IRequest<QuizReultDto>
    {
        public string QuizCode { get; set; }
    }

    public class GetQuizResultQueryHandler : IRequestHandler<GetExaminerQuizResultQuery, QuizReultDto>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;


        public GetQuizResultQueryHandler(IApplicationRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;

        }

        public async Task<QuizReultDto> Handle(GetExaminerQuizResultQuery request, CancellationToken cancellationToken)
        {

            var quizResult = await _repository.QuizResults.GetUserQuizResult(_currentUserService.UserId, request.QuizCode);

            if (quizResult == null)
                throw new QuizNotStartedException("Quiz not started or quiz code is not exist");

            if (!quizResult.EndDate.HasValue)
                throw new QuizNotSbmitted();

            return MapQuizResult(quizResult);
        }

        private QuizReultDto MapQuizResult(QuizResult quizResult)
        {
            QuizReultDto result = new QuizReultDto();

            result.Code = quizResult.Quiz.Code;
            result.Title = quizResult.Quiz.Title;
            result.Score = quizResult.Score;
            result.Questions = quizResult.QuizResultQuestions.Select(q => new QuestionResultDto
            { Description = q.Question.Description, Score = q.Score }).ToList();

            return result;
        }
    }
}