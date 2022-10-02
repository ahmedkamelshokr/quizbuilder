namespace Application.Quizs.Commands
{
    public class PublishQuizCommand : IRequest
    {
        public string QuizCode { get; set; }
    }

    public class PublishQuizCommandHandler : IRequestHandler<PublishQuizCommand>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public PublishQuizCommandHandler(IApplicationRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(PublishQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.Quizs.GetByCode(request.QuizCode);

            Validate(quiz);

            quiz.Publish();

            await _repository.SaveChangesAsync();
            return Unit.Value;
        }

        private void Validate(Quiz quiz)
        {
            if (quiz == null)
                throw new NotFoundException();

            if (!quiz.OwnedByUser(_currentUserService.UserId))
                throw new UnauthorizedAccessException();
        }
    }
}
