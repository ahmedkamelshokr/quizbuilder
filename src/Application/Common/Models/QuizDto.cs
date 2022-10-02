namespace Application.Common.Models
{

    public class QuizDto
    {
        public string Code { get; set; }
        public string Title { get; set; }

        public IEnumerable<RQuestionDto> Questions { get; set; }
    }
    public class EQuizDto
    {
        public string Code { get; set; }
        public string Title { get; set; }

        public IEnumerable<EQuestionDto> Questions { get; set; }
    }
    public class PublishedQuizDto
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public DateTime PublisheDate { get; set; }
        public string Publisher { get; set; }
    }
}
