namespace Application.Quizs.Queries
{
    public class GetQuizResultStatsQuery : IRequest<QuizReultStatistics>
    {
        public string QuizCode { get; set; }
    }

    public class GetQuizResultStatsQueryQueryHandler : IRequestHandler<GetQuizResultStatsQuery, QuizReultStatistics>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;


        public GetQuizResultStatsQueryQueryHandler(IApplicationRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;

        }

        public async Task<QuizReultStatistics> Handle(GetQuizResultStatsQuery request, CancellationToken cancellationToken)
        {
            await Validate(request);

            var quizResults = await _repository.QuizResults.Where(q => q.Quiz.Code == request.QuizCode && q.EndDate != null).ToListAsync();

            if (quizResults == null || !quizResults.Any())
            {
                var quiz = await _repository.Quizs.GetByCode(request.QuizCode);
                return new QuizReultStatistics { Code = request.QuizCode, ExaminersCount = 0, Title = quiz.Title };
            }

            return MapQuizResults(quizResults);
        }

        private QuizReultStatistics MapQuizResults(List<QuizResult>? quizResults)
        {
            var result = new QuizReultStatistics
            {
                Code = quizResults?.FirstOrDefault().Quiz.Code,
                Title = quizResults?.FirstOrDefault().Quiz.Title,
                ExaminersCount = quizResults.Count
            };


            var scores = quizResults.Select(qr => qr.Score).ToList();

            //TODO calculate as % 
            for (int i = 0; i < 100; i += 10)
            {
                if (scores.Any(s => s >= i && s < i + 10))

                    result.Scores.Add(new ScoreRanges
                    {
                        From = i,
                        To = i + 10,
                        Count = scores.Count(s => s >= i && s < i + 10)
                    });
            }

            return result;
        }

        private async Task Validate(GetQuizResultStatsQuery request)
        {
            var quiz = await _repository.Quizs.GetByCode(request.QuizCode);

            if (quiz == null)
                throw new NotFoundException();

            if (!quiz.OwnedByUser(_currentUserService.UserId))
                throw new UnauthorizedAccessException();

            if (!quiz.Published)
                throw new QuizIsNotPublishedException(quiz.Code);
        }
    }
}