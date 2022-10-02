

namespace Domain.Entities
{
    public class Quiz : Entity
    {

        #region Properties
        [Required]
        [StringLength(6)]
        public string Code { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }
        public bool Published { get; set; }
        public DateTime? PublisheDate { get; set; }
        public bool Deleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public virtual List<Question> Questions { get; set; }

        public virtual User User { get; set; }
        #endregion


        public Quiz(string title)
        {
            Guard.Against.NullOrEmpty(title, nameof(title));

            Code = NanoRandomNumberGenerator.GenerateNew(5);
            Title = title;

            Questions = new List<Question>();
        }

        public void AddQuestion(Question question)
        {

            ValidateQuestion(question);
            SetQuestionAnswerScores(question);
            Questions.Add(question);
        }

        public void RemoveQuestionById(long questionId)
        {
            var question = Questions.SingleOrDefault(q => q.Id == questionId);
            if (question == null)
                throw new EntityNotFoundException($"Question Id {questionId} is not found");

            Questions.Remove(question);
        }

        public void Publish()
        {
            Published = true;
            PublisheDate = DateTime.Now;
        }

        public void Delete()
        {
            Deleted = true;
            DeletedDate = DateTime.Now;
        }
        public bool OwnedByUser(string userId)
        {
            return User?.Id == userId;
        }

        private static void ValidateQuestion(Question question)
        {
            Guard.Against.Null(question, nameof(question));

            if (question.Answers.Count > 5 ||
                !question.Answers.Any()
                || !question.Answers.Any(a => a.IsCorrect.GetValueOrDefault())
            || (question.Questiontype == QuestionType.SingleCorrectAnswer && question.Answers.Count(a => a.IsCorrect.GetValueOrDefault()) != 1)
                )
                throw new InvalidQuestionAnswers(question.Description);

        }

        private static void SetQuestionAnswerScores(Question question)
        {
            Guard.Against.Null(question, nameof(question));
            var correctAnswersCount = question.Answers.Count(a => a.IsCorrect.GetValueOrDefault());
            var wrongAnswersCount = question.Answers.Count() - correctAnswersCount;
            question.Answers.Where(a => a.IsCorrect.GetValueOrDefault()).ToList().ForEach(a => a.Score = 1.0m / correctAnswersCount);
            question.Answers.Where(a => !a.IsCorrect.GetValueOrDefault()).ToList().ForEach(a => a.Score = -1.0m / wrongAnswersCount);

        }


    }

}
