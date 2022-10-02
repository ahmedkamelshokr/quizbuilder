namespace Application.Common.Models
{

    public class QuizAnswersDto
    {
        public int QuestionId { get; set; }
        public List<long> SelectedAnswers { get; set; }

    }
}
