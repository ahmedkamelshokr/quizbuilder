namespace Domain.UnitTests.Entites
{
    public class QuestionTest
    {
        private Question _question;

        [SetUp]
        public void SetUp()
        {
            _question = new Question("Question # 1", Enums.QuestionType.SingleCorrectAnswer);
        }


        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ShouldThrowException_WhenDescriptionIsNullOrEmpty(string description)
        {
            Action action = () => new Question(description, Enums.QuestionType.SingleCorrectAnswer);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*description*");
        }

        [Test]
        public void ShouldThrowException_WhenQuestionTypeIsNull()
        {
            Action action = () => new Answer("test answer", null);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*isCorrect*");
        }
        [Test]
        public void AddingAnswer_ShouldThrowException_WhenAnswerIsNull()
        {
            _question.Invoking(q => q.AddAnswer(null))
               .Should().Throw<ArgumentException>();
        }
    }
}
