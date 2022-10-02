namespace Application.Quizs.Queries
{
    public class GetQuizForEditQuery : IRequest<EQuizDto>
    {
        public string QuizCode { get; set; }
    }

    public class GetQuizForEditQueryHandler : IRequestHandler<GetQuizForEditQuery, EQuizDto>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetQuizForEditQueryHandler(IApplicationRepository repository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<EQuizDto> Handle(GetQuizForEditQuery request, CancellationToken cancellationToken)
        {

            var quiz = await _repository.Quizs
               .Where(q => q.Code == request.QuizCode)
               .Include(q => q.Questions)
               .ThenInclude(qes => qes.Answers)
               .FirstOrDefaultAsync();

            if (quiz == null)
                throw new NotFoundException();

            if (!quiz.OwnedByUser(_currentUserService.UserId))
                throw new UnauthorizedAccessException();

            var result = _mapper.Map<EQuizDto>(quiz);

            return result;
        }
    }
}