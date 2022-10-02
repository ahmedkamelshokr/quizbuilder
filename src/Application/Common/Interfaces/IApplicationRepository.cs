namespace Application.Common.Interfaces
{
    public interface IApplicationRepository
    {
        DbSet<Quiz> Quizs { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<QuizResult> QuizResults { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
    }
}
