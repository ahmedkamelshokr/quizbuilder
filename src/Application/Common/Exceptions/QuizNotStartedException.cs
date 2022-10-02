namespace Application.Common.Exceptions
{
    public class QuizNotStartedException : Exception
    {
        public QuizNotStartedException() : base("User should start quiz first!")
        {

        }
        public QuizNotStartedException(string message) : base(message)
        {

        }
    }

    public class QuizIsPublishedException : Exception
    {
        public QuizIsPublishedException(string quizCode) : base($"Quiz {quizCode} is published and can not be edited!")
        {

        }
    }

    public class QuizIsNotPublishedException : Exception
    {
        public QuizIsNotPublishedException(string quizCode) : base($"Quiz {quizCode} is not published and can not be started!")
        {

        }
    }
    public class QuizIsDeletedException : Exception
    {
        public QuizIsDeletedException(string quizCode) : base($"Quiz {quizCode} is deleted and can not be started!")
        {

        }
    }

    public class QuizAlreadyStartedException : Exception
    {
        public QuizAlreadyStartedException(string quizCode) : base($"Quiz {quizCode} already started!")
        {

        }
    }

    public class QuizAlreadySbmitted : Exception
    {
        public QuizAlreadySbmitted() : base("User submitted this quiz before!")
        {

        }
    }
    public class QuizNotSbmitted : Exception
    {
        public QuizNotSbmitted() : base("User did not submit this quiz before!")
        {

        }
    }
}
