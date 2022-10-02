namespace Application.Common.Models
{
    public class QuizReultStatistics
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int ExaminersCount { get; set; }
        public List<ScoreRanges> Scores { get; set; }=new List<ScoreRanges>();
    }

    public class ScoreRanges
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Count { get; set; }
    }
}
