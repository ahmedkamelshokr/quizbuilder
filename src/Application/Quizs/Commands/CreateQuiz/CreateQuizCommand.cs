namespace Application.Quizs.Commands
{
    public class CreateQuizCommand : IRequest<string>
    {
        public string Title { get; set; }

        public IEnumerable<CQuestionDto> Questions { get; set; }
    }

    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, string>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateQuizCommandHandler(IApplicationRepository repository,
            ICurrentUserService currentUserService,
              IMapper mapper)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = new Quiz(request.Title);

            var quizOwner = await _repository.Users.GetById(_currentUserService.UserId);

            quiz.User = quizOwner;

            request.Questions?.ToList().ForEach(q =>
            {
                var question = _mapper.Map<Question>(q);
                quiz.AddQuestion(question);

            });

            _repository.Quizs.Add(quiz);

            await _repository.SaveChangesAsync(cancellationToken);

            return quiz.Code;
        }
    }
}
