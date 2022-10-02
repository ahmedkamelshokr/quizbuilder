namespace Application.Quizs.Commands
{
    public class RemoveQuestionFromQuizCommand : IRequest<string>
    {
        public string QuizCode { get; set; }

        public List<long> QuestionsIds { get; set; }
    }

    public class RemoveQuestionFromQuizCommandHandler : IRequestHandler<RemoveQuestionFromQuizCommand, string>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public RemoveQuestionFromQuizCommandHandler(IApplicationRepository repository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<string> Handle(RemoveQuestionFromQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.Quizs.GetByCode(request.QuizCode);

            Validate(request, quiz);

            request.QuestionsIds.ForEach(qid =>
            {
                quiz.RemoveQuestionById(qid);
            });


            await _repository.SaveChangesAsync(cancellationToken);

            return quiz.Code;
        }

        private void Validate(RemoveQuestionFromQuizCommand request, Quiz quiz)
        {
            if (quiz == null)
                throw new NotFoundException();

            if (quiz.Published)
                throw new QuizIsPublishedException(request.QuizCode);


            if (!quiz.OwnedByUser(_currentUserService.UserId))
                throw new UnauthorizedAccessException();
        }
    }
}
