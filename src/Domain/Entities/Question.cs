 
namespace Domain.Entities
{
    public class Question : Entity
    {
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public QuestionType? Questiontype { get; set; }

        public virtual List<Answer> Answers { get; set; }

        public virtual Quiz Quiz { get; set; }

        public Question(string description, QuestionType? questiontype)
        {
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.Null(questiontype, nameof(questiontype));

            Description = description;
            Questiontype = questiontype;
            Answers = new List<Answer>();
        }

        public void AddAnswer(Answer answer)
        {
            Guard.Against.Null(answer, nameof(answer));
            Answers.Add(answer);
        }
    }

}
