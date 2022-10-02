

namespace Application.UnitTests.Quizs.Commands
{
    public class CreateQuizCommandTests : TestBase
    {

        [Test]
        public async Task ShouldSaveQuizToRepository()
        {
            var quizTitle = "First Quiz";

            var question = new CQuestionDto() { Description = "test question", Questiontype = Domain.Enums.QuestionType.SingleCorrectAnswer };
            question.Answers.Add(new CAnswerDto { Description = "correct answer", IsCorrect = true });
            question.Answers.Add(new CAnswerDto { Description = "wrong answer", IsCorrect = false });


            var command = new CreateQuizCommand { Title = quizTitle, Questions = new List<CQuestionDto> { question } };
            var code = await SendAsync(command);

            var quiz = await Repository.Quizs.GetByCode(code);
            quiz.Should().NotBeNull();
            quiz.Questions.Count().Should().Be(1);
            quiz.Title.Should().Be(quizTitle);
            quiz.Code.Should().NotBeNull();

        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowException_WhenTitleIsNullOrEmpty(string title)
        {
            var command = new CreateQuizCommand { Title = title };
            await Awaiting(() => SendAsync(command))
                            .Should().ThrowAsync<ValidationException>();
        }


        [Test]
        public async Task ShouldThrowInvalidQuestionAnswers_WhenAnyQuestionHasInvalidAnswers()
        {
            var question = new CQuestionDto() { Description = "test question", Questiontype = Domain.Enums.QuestionType.SingleCorrectAnswer };
            question.Answers.Add(new CAnswerDto { Description = "correct answer", IsCorrect = true });
            question.Answers.Add(new CAnswerDto { Description = "wrong answer", IsCorrect = true });

            var command = new CreateQuizCommand { Title = "test quiz", Questions = new List<CQuestionDto> { question } };

            await Awaiting(() => SendAsync(command))
                  .Should().ThrowAsync<InvalidQuestionAnswers>();
        }
    }
}
