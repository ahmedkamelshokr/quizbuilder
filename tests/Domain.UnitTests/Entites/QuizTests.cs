namespace Domain.UnitTests.Entites
{
    public class QuizTests
    {
        private Quiz _quiz;

        [SetUp]
        public void SetUp()
        {
            _quiz = new Quiz("Quiz # 1");
        }


        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ShouldThrowException_WhenTitleIsNullOrEmpty(string title)
        {
            Action action = () => new Quiz(title);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*title*");
        }

        [Test]
        public void ShouldAssignQuizCodeWhenCreatingNewQuiz()
        {
            var quiz = new Quiz("Quiz # 1");
            quiz.Code.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void AddingQuestion_ShouldThrowException_WhenQuestionIsNull()
        {
            _quiz.Invoking(q => q.AddQuestion(null))
               .Should().Throw<ArgumentException>();
        }

        [Test]
        public void Publish_ShouldSetPublishedAndPublishDate()
        {
            _quiz.Publish();

            _quiz.Published.Should().BeTrue();
            Assert.That(_quiz.PublisheDate, Is.EqualTo(DateTime.Now).Within(1).Minutes);

        }

        [Test]
        public void Delete_ShouldSetDeleteddAndDeleteDate()
        {
            _quiz.Delete();

            _quiz.Deleted.Should().BeTrue();
            Assert.That(_quiz.DeletedDate, Is.EqualTo(DateTime.Now).Within(1).Minutes);

        }

        [Test]
        [TestCase(QuestionType.SingleCorrectAnswer)]
        [TestCase(QuestionType.MultibleCorrectAnswers)]
        public void AddingQuestion_ShouldThrowException_WhenQuestionHasNoAnswers(QuestionType questionType)
        {

            var invalidQuestion = new Question("Question #1 ", questionType);

            _quiz.Invoking(q => q.AddQuestion(invalidQuestion))
               .Should().Throw<InvalidQuestionAnswers>();
        }

        [Test]
        [TestCase(QuestionType.SingleCorrectAnswer)]
        [TestCase(QuestionType.MultibleCorrectAnswers)]
        public void AddingQuestion_ShouldThrowException_WhenQuestionHasNoCorrectAnswers(QuestionType questionType)
        {

            var invalidQuestion = new Question("Question #1 ", questionType);

            invalidQuestion.AddAnswer(new Answer("Answer # 1", false));
            invalidQuestion.AddAnswer(new Answer("Answer # 2", false));

            _quiz.Invoking(q => q.AddQuestion(invalidQuestion))
               .Should().Throw<InvalidQuestionAnswers>();
        }

        [Test]
        public void AddingQuestion_ShouldThrowException_WhenSingleCorrectAnswerQuestionHasMoreThanCorrectAnswers()
        {

            var invalidQuestion = new Question("Question #1 ", QuestionType.SingleCorrectAnswer);
            invalidQuestion.AddAnswer(new Answer("Answer # 1", true));
            invalidQuestion.AddAnswer(new Answer("Answer # 2", true));

            _quiz.Invoking(q => q.AddQuestion(invalidQuestion))
               .Should().Throw<InvalidQuestionAnswers>();
        }

    }
}
