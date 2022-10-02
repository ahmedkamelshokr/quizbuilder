namespace Application.Quizs.Queries
{
    public class GetQuizForExaminerQuery : IRequest<QuizDto>
    {
        public string QuizCode { get; set; }
    }

    public class GetQuizQueryHandler : IRequestHandler<GetQuizForExaminerQuery, QuizDto>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetQuizQueryHandler(IApplicationRepository repository,
            ICurrentUserService currentUserService,
              IMapper mapper)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<QuizDto> Handle(GetQuizForExaminerQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.Quizs
                .Where(q => q.Code == request.QuizCode)
                .Include(q => q.Questions)
                .ThenInclude(qes => qes.Answers)
                .FirstOrDefaultAsync();

            await Validate(quiz);

            var result = _mapper.Map<QuizDto>(quiz);

            return result;
        }

        private async Task Validate(Quiz quiz)
        {
            if (quiz == null)
                throw new NotFoundException();

            var quizResult = await _repository.QuizResults.GetUserQuizResult(_currentUserService.UserId, quiz.Code);

            if (quizResult == null)
                throw new QuizNotStartedException();

            if (quizResult.EndDate.HasValue)
                throw new QuizAlreadySbmitted();
        }
    }
}