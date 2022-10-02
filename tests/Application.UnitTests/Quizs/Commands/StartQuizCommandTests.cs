namespace Application.UnitTests.Quizs.Commands
{
    public class StartQuizCommandTests : TestBase
    {
        private string quizCode;

        [SetUp]
        public void Setup()
        {
            CreatQuiz();
        }

        [Test]
        public async Task ShouldSaveQuizResultToRepository()
        {
            var command = new StartQuizCommand { QuizCode = quizCode };
            var code = await SendAsync(command);

            var quizResult = await Repository.QuizResults.GetUserQuizResult(quizeOwner.Id, code);
            quizResult.Should().NotBeNull();
            quizResult.Score.Should().Be(0);
            Assert.That(quizResult.StartDate, Is.EqualTo(DateTime.Now).Within(1).Minutes);
        }


        [Test]
        public async Task ShouldThrowException_WhenQuizIsNotPublished()
        {
            var quiz = await Repository.Quizs.GetByCode(quizCode);
            quiz.Published = false;
            await Repository.SaveChangesAsync();
            var command = new StartQuizCommand { QuizCode = quizCode };

            await Awaiting(() => SendAsync(command))
                  .Should().ThrowAsync<QuizIsNotPublishedException>();
        }

        [Test]
        public async Task ShouldThrowException_WhenQuizCodeNotFound()
        {
            ;
            var command = new StartQuizCommand { QuizCode = "notfound" };

            await Awaiting(() => SendAsync(command))
                  .Should().ThrowAsync<NotFoundException>();
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
            quiz.Publish();
            Repository.Quizs.Add(quiz);
            await Repository.SaveChangesAsync();
            quizCode = quiz.Code;
        }
    }
}
