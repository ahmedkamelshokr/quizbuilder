namespace Domain.UnitTests.Entites
{
    public class AnswerTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ShouldThrowException_WhenDescriptionIsNullOrEmpty(string description)
        {
            Action action = () => new Answer(description, true);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*description*");
        }

        [Test]
        public void ShouldThrowException_WhenIsCorrectsNull()
        {
            Action action = () => new Answer("test answer", null);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*isCorrect*");
        }
    }
}
