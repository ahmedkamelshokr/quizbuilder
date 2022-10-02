
namespace Domain.Entities
{
    public class Answer : Entity
    {
        public virtual Question Question { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public bool? IsCorrect { get; set; }

        public decimal? Score { get; set; }

        public Answer(string description, bool? isCorrect)
        {
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.Null(isCorrect, nameof(isCorrect));

            Description = description;
            IsCorrect = isCorrect;
        }
    }

}
