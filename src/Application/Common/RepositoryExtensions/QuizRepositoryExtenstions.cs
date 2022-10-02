namespace Application.Common.RepositoryExtensions
{
    public static class QuizRepositoryExtenstions
    {
        public static async Task<Quiz> GetByCode(this DbSet<Quiz> repo, string code)
        {
            return string.IsNullOrWhiteSpace(code)
                 ? null
                 : await repo.FirstOrDefaultAsync(q => q.Code == code);
        }
    }
}
