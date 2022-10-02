

using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class QuizResultQuestions : Entity
    {
        public QuizResultQuestions()
        {
            QuizResultQuestionsAnswers = new List<QuizResultQuestionsAnswer>();
        }
        public virtual QuizResult QuizResult { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
        public long QuestionId { get; set; }
        public decimal Score { get; set; }

        public virtual List<QuizResultQuestionsAnswer> QuizResultQuestionsAnswers { get; set; }
    }

}
