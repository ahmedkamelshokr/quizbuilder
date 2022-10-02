

namespace Application.UnitTests.Quizs.Commands
{
    public class UpdateQuizCommandTests : TestBase
    {

        private string quizCode;

        [SetUp]
        public void Setup()
        {
            CreatQuiz();
        }

        [Test]
        public async Task ShouldSaveQuizToRepository()
        {
            var quizTitle = "updated Quiz";

            var command = new UpdateQuizCommand { QuizCode = quizCode, Title = quizTitle };
            var code = await SendAsync(command);

            var quiz = await Repository.Quizs.GetByCode(code);
            quiz.Should().NotBeNull();
            quiz.Questions.Count().Should().Be(1);
            quiz.Title.Should().Be(quizTitle);
            quiz.Code.Should().NotBeNull();

        }

        [Test]
        public async Task QuizCreatedByDifferentUser_Should_ThrowUnAuthorizedException()
        {

            MockCurrentUserService.SetupGet(x => x.UserId).Returns("123"); //different user id
            var quizTitle = "updated Quiz";

            var command = new UpdateQuizCommand { QuizCode = quizCode, Title = quizTitle };

            await Awaiting(() => SendAsync(command))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowException_WhenTitleIsNullOrEmpty(string title)
        {
            var command = new UpdateQuizCommand { QuizCode = quizCode, Title = title };

            await Awaiting(() => SendAsync(command))
                 .Should().ThrowAsync<ValidationException>();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowException_WhenCodeIsNullOrEmpty(string code)
        {
            var command = new UpdateQuizCommand { QuizCode = code, Title = " test quiz" };

            await Awaiting(() => SendAsync(command))
                 .Should().ThrowAsync<ValidationException>();
        }


        [Test]
        public async Task ShouldThrowException_WhenCodeNotExist()
        {
            var command = new UpdateQuizCommand { QuizCode = "notfound", Title = " test quiz" };

            await Awaiting(() => SendAsync(command))
                 .Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowException_WhenQuizIsPublished()
        {
            var quiz = await Repository.Quizs.GetByCode(quizCode);
            quiz.Publish();
            await Repository.SaveChangesAsync();
            var command = new UpdateQuizCommand { QuizCode = quizCode, Title = "test quize" };

            await Awaiting(() => SendAsync(command))
                  .Should().ThrowAsync<QuizIsNotPublishedException>();
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
