namespace Application.Common.Models
{
    public class QuizReultDto
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public decimal Score { get; set; }
        public IEnumerable<QuestionResultDto> Questions { get; set; }=new List<QuestionResultDto>();
    }
}
