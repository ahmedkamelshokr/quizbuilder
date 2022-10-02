namespace Application.Common.RepositoryExtensions
{
    public static class QuizResultRepositoryExtenstions
    {
        public static async Task<QuizResult> GetUserQuizResult(this DbSet<QuizResult> repo, string userId, string code)
        {
            return string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(userId)
                 ? null
                 : await repo.FirstOrDefaultAsync(q => q.Quiz.Code == code && q.User.Id == userId);
        }
    }
}