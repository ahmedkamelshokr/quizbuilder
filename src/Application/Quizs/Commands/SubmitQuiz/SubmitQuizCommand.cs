namespace Application.Quizs.Commands
{
    public class SubmitQuizCommand : IRequest<string>
    {
        public string QuizCode { get; set; }
        public List<QuizAnswersDto> QuizQuestions { get; set; }

    }

    public class SubmitQuizCommandHandler : IRequestHandler<SubmitQuizCommand, string>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public SubmitQuizCommandHandler(IApplicationRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(SubmitQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.Quizs.GetByCode(request.QuizCode);

            if (quiz == null)
                throw new NotFoundException();

            var quizResult = await _repository.QuizResults.GetUserQuizResult(_currentUserService.UserId, request.QuizCode);

            ValidateQuizResult(quizResult);

            ValidateAnswers(quiz, request.QuizQuestions);


            UpdateQuizResult(request, quizResult, quiz);

            await _repository.SaveChangesAsync(cancellationToken);

            return quiz.Code;
        }

        private static void ValidateQuizResult(QuizResult quizResult)
        {
            if (quizResult == null)
                throw new QuizNotStartedException();

            if (quizResult.EndDate.HasValue)
                throw new QuizAlreadySbmitted();
        }

        private static void UpdateQuizResult(SubmitQuizCommand request, QuizResult quizResult, Quiz quiz)
        {
            var validQuizAnswers = quiz.Questions.SelectMany(q => q.Answers).ToList();

            var userSelectedAnswers = request.QuizQuestions.SelectMany(ua => ua.SelectedAnswers).ToList();

            var score = validQuizAnswers.Where(va => userSelectedAnswers.Contains(va.Id)).Sum(va => va.Score);

            quizResult.Score = score.GetValueOrDefault();
            quizResult.EndDate = DateTime.Now;

            foreach (var q in request.QuizQuestions)
            {
                var quizResultQuestions = new QuizResultQuestions();
                quizResultQuestions.QuestionId = q.QuestionId;
                quizResultQuestions.QuizResultQuestionsAnswers.AddRange(
                    q.SelectedAnswers.Select(sa => new QuizResultQuestionsAnswer { AnswerId = sa }));

                quizResultQuestions.Score = validQuizAnswers.Where(va => q.SelectedAnswers.Contains(va.Id)).Sum(va => va.Score).GetValueOrDefault();

                quizResult.QuizResultQuestions.Add(quizResultQuestions);
            }
        }

        private void ValidateAnswers(Quiz quiz, List<QuizAnswersDto> QuizQuestions)
        {
            var quizQuestionsIds = quiz.Questions.Select(q => q.Id);

            //dto contains questions do not belong to this quiz
            if (QuizQuestions.Any(qq => !quizQuestionsIds.Contains(qq.QuestionId)))
                throw new InvalidPayloadException("Payload contains questions do not belong to this quiz");

            //single answer question has more than one answer
            var singleAnswerQuestions = quiz.Questions.
                Where(q => q.Questiontype == Domain.Enums.QuestionType.SingleCorrectAnswer)
                .Select(q => q.Id).ToList();
            if (QuizQuestions.Any(qq => singleAnswerQuestions.Contains(qq.QuestionId) && qq.SelectedAnswers.Count > 1))
                throw new InvalidPayloadException("single answer question MUST have only one answer");

            var validQuizAnswers = quiz.Questions.SelectMany(q => q.Answers).ToList();

            var userSelectedAnswers = QuizQuestions.SelectMany(ua => ua.SelectedAnswers);
            //dto contains answers do not belong to this quiz
            if (userSelectedAnswers.Any(sa => !validQuizAnswers.Any(va => va.Id == sa)))
                throw new InvalidPayloadException("Payload has invalid answers");


        }
    }
}
