namespace Application.Common.Models
{
    public class CAnswerDto
    {
        public string Description { get; set; }

        public bool? IsCorrect { get; set; }
    }

    public class RAnswerDto
    {
        public long Id { get; set; }
        public string Description { get; set; }

    }

    public class EAnswerDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }

    }
}
