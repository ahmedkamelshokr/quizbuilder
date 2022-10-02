namespace Application.Quizs.Commands
{
    public class StartQuizCommand : IRequest<string>
    {
        public string QuizCode { get; set; }

    }

    public class StartQuizCommandHandler : IRequestHandler<StartQuizCommand, string>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public StartQuizCommandHandler(IApplicationRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(StartQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.Quizs.GetByCode(request.QuizCode);
            await ValidateAsync(quiz);

            var user = await _repository.Users.GetById(_currentUserService.UserId);

            var quizResult = new QuizResult(quiz, user);
            quizResult.StartQuiz();
            _repository.QuizResults.Add(quizResult);

            await _repository.SaveChangesAsync(cancellationToken);

            return quiz.Code;
        }

        private async Task<Quiz> ValidateAsync(Quiz quiz)
        {
            if (quiz == null)
                throw new NotFoundException();

            if (!quiz.Published)
                throw new QuizIsNotPublishedException(quiz.Code); 

            if (quiz.Deleted)
                throw new QuizIsDeletedException(quiz.Code);

            var alreadyStarted = await _repository.QuizResults
                .AnyAsync(qr => qr.User.Id == _currentUserService.UserId && qr.Quiz.Code == quiz.Code);

            if (alreadyStarted)
                throw new QuizAlreadyStartedException(quiz.Code);

            return quiz;
        }
    }
}
