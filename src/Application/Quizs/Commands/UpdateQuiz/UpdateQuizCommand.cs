namespace Application.Quizs.Commands
{
    public class UpdateQuizCommand : IRequest<string>
    {
        public string QuizCode { get; set; }
        public string Title { get; set; }
    }

    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, string>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateQuizCommandHandler(IApplicationRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.Quizs.GetByCode(request.QuizCode);

            Validate(request, quiz);

            quiz.Title = request.Title;

            await _repository.SaveChangesAsync(cancellationToken);

            return quiz.Code;
        }

        private void Validate(UpdateQuizCommand request, Quiz quiz)
        {
            if (quiz == null)
                throw new NotFoundException();

            if (quiz.Published)
                throw new QuizIsNotPublishedException(request.QuizCode);

            if (!quiz.OwnedByUser(_currentUserService.UserId))
                throw new UnauthorizedAccessException();
        }
    }
}
