using SharedKernel;

namespace Application.UnitTests
{
    public class InMemoryRepository : DbContext, IApplicationRepository
    {
        public DbSet<Quiz> Quizs { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<QuizResult> QuizResults { get; set; }
        public InMemoryRepository(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(EntityConfigurations).Assembly);

            base.OnModelCreating(builder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
