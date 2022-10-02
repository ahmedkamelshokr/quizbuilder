
namespace Application.UnitTests.Quizs.Commands
{
    public class DeleteQuizCommandTests : TestBase
    {
        private string quizCode;

        [SetUp]
        public void Setup()
        {
            CreatQuiz();
        }


        [Test]
        public async Task ShouldDeleteQuizInRepository()
        {
            var command = new DeleteQuizCommand { QuizCode = quizCode };
            await SendAsync(command);

            var quiz = await Repository.Quizs.GetByCode(quizCode);
            quiz.Should().NotBeNull();
            quiz.Deleted.Should().BeTrue();
            Assert.That(quiz.DeletedDate, Is.EqualTo(DateTime.Now).Within(1).Minutes);
        }

        [Test]
        public async Task QuizCodeNotFound_Should_ThrowNotFoundException()
        {

            var command = new DeleteQuizCommand { QuizCode = "notfoud" };

            await Awaiting(() => SendAsync(command))
                .Should().ThrowAsync<NotFoundException>();

        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowValidationExcptionWhenCodeIsEmpty(string code)
        {

            var command = new DeleteQuizCommand { QuizCode = code };

            await Awaiting(() => SendAsync(command))
               .Should().ThrowAsync<ValidationException>();

        }

        [Test]
        public async Task QuizCreatedByDifferentUser_Should_ThrowUnAuthorizedException()
        {

            MockCurrentUserService.SetupGet(x => x.UserId).Returns("123"); //different user id
            var command = new DeleteQuizCommand { QuizCode = quizCode };

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
