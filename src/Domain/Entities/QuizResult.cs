namespace Domain.Entities
{
    public class QuizResult : Entity
    {
        protected QuizResult()
        {

        }
        public QuizResult(Quiz quiz, User user)
        {
            Guard.Against.Null(quiz, nameof(quiz));
            Guard.Against.Null(user, nameof(user));

            Quiz = quiz;
            User = user;
        }
        public virtual User User { get; set; }

        public virtual Quiz Quiz { get; set; }

        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; set; }
        public decimal Score { get; set; }

        public virtual List<QuizResultQuestions> QuizResultQuestions { get; set; }
        public void StartQuiz()
        {
            StartDate = DateTime.Now;
        }


    }

}
