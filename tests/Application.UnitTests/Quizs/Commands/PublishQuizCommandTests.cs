namespace Application.UnitTests.Quizs.Commands
{
    public class PublishQuizCommandTests : TestBase
    {
        private string quizCode;

        [SetUp]
        public void Setup()
        {
            CreatQuiz();
        }


        [Test]
        public async Task ShouldPublishQuizInRepository()
        {
            var command = new PublishQuizCommand { QuizCode = quizCode };
            await SendAsync(command);

            var quiz = await Repository.Quizs.GetByCode(quizCode);
            quiz.Should().NotBeNull();
            quiz.Published.Should().BeTrue();
            Assert.That(quiz.PublisheDate, Is.EqualTo(DateTime.Now).Within(1).Minutes);
        }

        [Test]
        public async Task QuizCodeNotFound_Should_ThrowNotFoundException()
        {

            var command = new PublishQuizCommand { QuizCode = "notfoud" };

            await Awaiting(() => SendAsync(command))
                 .Should().ThrowAsync<NotFoundException>();

        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowValidationExcptionWhenCodeIsEmpty(string code)
        {

            var command = new PublishQuizCommand { QuizCode = code };

            await Awaiting(() => SendAsync(command))
                 .Should().ThrowAsync<ValidationException>();

        }

        [Test]
        public async Task QuizCreatedByDifferentUser_Should_ThrowUnAuthorizedException()
        {

            MockCurrentUserService.SetupGet(x => x.UserId).Returns("123"); //different user id
            var command = new PublishQuizCommand { QuizCode = quizCode };

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
            quizCode = quiz.Code;
        }
    }
}
