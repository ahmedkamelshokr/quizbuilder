namespace Application.UnitTests.Quizs.Commands
{
    public class AddQuestionToQuizCommandTests : TestBase
    {
        private string quizCode;

        private CQuestionDto testQuestion;

        [SetUp]
        public void Setup()
        {
            CreatQuiz();

            testQuestion = new CQuestionDto { Description = "test question 2", Questiontype = Domain.Enums.QuestionType.SingleCorrectAnswer };
            testQuestion.Answers.Add(new CAnswerDto { Description = "correct answer", IsCorrect = true });
            testQuestion.Answers.Add(new CAnswerDto { Description = "wrong answer", IsCorrect = false });

        }

        [Test]
        public async Task QuizCodeNotFound_Should_ThrowNotFoundException()
        {

            var command = new AddQuestionToQuizCommand { QuizCode = "notfoud", Questions =new List<CQuestionDto> { testQuestion } };

            await Awaiting(() => SendAsync(command))
                 .Should().ThrowAsync<NotFoundException>();

        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowValidationExcptionWhenCodeIsEmpty(string code)
        {

            var command = new AddQuestionToQuizCommand { QuizCode = code, Questions = new List<CQuestionDto> { testQuestion } };

            await Awaiting(() => SendAsync(command))
                  .Should().ThrowAsync<ValidationException>();

        }

        [Test]
        public async Task ShouldThrowValidationExcptionWhenQuestionIsNull()
        {

            var command = new AddQuestionToQuizCommand { QuizCode = quizCode };

            await Awaiting(() => SendAsync(command))
                .Should().ThrowAsync<ValidationException>();

        }


        [Test]
        public async Task QuizCreatedByDifferentUser_Should_ThrowUnAuthorizedException()
        {

            MockCurrentUserService.SetupGet(x => x.UserId).Returns("123"); //different user id
            var command = new AddQuestionToQuizCommand { QuizCode = quizCode, Questions = new List<CQuestionDto> { testQuestion } };

            await Awaiting(() => SendAsync(command))
                  .Should().ThrowAsync<UnauthorizedAccessException>();
        }

        private async void CreatQuiz()
        {
            var quizTitle = "First Quiz";

            var question = new Question("test question", Domain.Enums.QuestionType.SingleCorrectAnswer);
            question.AddAnswer(new Answer("correct answer", true));
            question.AddAnswer(new Answer("wrong answer", false));
            var quiz = new Quiz(quizTitle);
            quiz.AddQuestion(question);

            quiz.User = quizeOwner;
            Repository.Quizs.Add(quiz);
            await Repository.SaveChangesAsync();
            MockCurrentUserService.SetupGet(x => x.UserId).Returns(quizeOwner.Id);
            quizCode = quiz.Code;
        }
    }
}
