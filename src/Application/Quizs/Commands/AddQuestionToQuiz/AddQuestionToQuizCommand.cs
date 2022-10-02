namespace Application.Quizs.Commands
{
    public class AddQuestionToQuizCommand : IRequest<string>
    {
        public string QuizCode { get; set; }

        public List<CQuestionDto> Questions { get; set; }
    }

    public class AddQuestionToQuizCommandHandler : IRequestHandler<AddQuestionToQuizCommand, string>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AddQuestionToQuizCommandHandler(IApplicationRepository repository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<string> Handle(AddQuestionToQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.Quizs.GetByCode(request.QuizCode);

            VaidateQuiz(quiz);

            request.Questions.ForEach(q =>
            {
                var question = _mapper.Map<Question>(q);

                quiz.AddQuestion(question);
            });


            await _repository.SaveChangesAsync(cancellationToken);

            return quiz.Code;
        }

        private void VaidateQuiz(Quiz quiz)
        {
            if (quiz == null)
                throw new NotFoundException();

            if (quiz.Published)
                throw new QuizIsPublishedException(quiz.Code);


            if (!quiz.OwnedByUser(_currentUserService.UserId))
                throw new UnauthorizedAccessException();
        }
    }
}
