using Domain.Enums;

namespace Application.Common.Models
{
    public class CQuestionDto
    {
        public string Description { get; set; }

        public QuestionType? Questiontype { get; set; }

        public virtual List<CAnswerDto> Answers { get; set; } = new List<CAnswerDto>();

    }

    public class RQuestionDto
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public QuestionType? Questiontype { get; set; }

        public virtual List<RAnswerDto> Answers { get; set; } = new List<RAnswerDto>();

    }
    public class EQuestionDto
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public QuestionType? Questiontype { get; set; }

        public virtual List<EAnswerDto> Answers { get; set; } = new List<EAnswerDto>();

    }
    public class QuestionResultDto
    {
        public string Description { get; set; }

        public decimal Score { get; set; }

    }
}
