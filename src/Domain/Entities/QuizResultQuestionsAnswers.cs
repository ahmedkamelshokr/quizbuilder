
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class QuizResultQuestionsAnswer : Entity
    {
        public virtual QuizResultQuestions QuizResultQuestion { get; set; }
        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; }
        public long AnswerId { get; set; }

    }

}
