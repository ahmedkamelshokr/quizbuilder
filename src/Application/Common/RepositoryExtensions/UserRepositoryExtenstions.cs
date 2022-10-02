namespace Application.Common.RepositoryExtensions
{
    public static class UserRepositoryExtenstions
    {
        public static async Task<User> GetById(this DbSet<User> repo, string userId)
        {
            return string.IsNullOrWhiteSpace(userId)
                 ? null
                 : await repo.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
