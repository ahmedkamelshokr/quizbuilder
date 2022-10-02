namespace Domain.Exceptions
{
    public class InvalidQuestionAnswers : Exception
    {
        public InvalidQuestionAnswers(string questionDescription) :
           base($"Question with description:{questionDescription} has invalid set of answers")
        {
        }
    }
}
