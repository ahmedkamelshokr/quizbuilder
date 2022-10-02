using Application.Common.Interfaces;
using Domain.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Infrastructure.Identity;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedKernel;

namespace Infrastructure.Persistence
{
    public class ApplicationRepository : ApiAuthorizationDbContext<ApplicationUser>, IApplicationRepository
    {

        public ApplicationRepository(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
            
        }

        public DbSet<Quiz> Quizs { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;

        }

        public void ClearChanges()
        {
            base.ChangeTracker.Clear();
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(EntityConfigurations).Assembly);
            builder.Entity<ApplicationUser>().HasOne(u => u.User).WithOne().OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            //builder.Entity<ApplicationRole>().HasOne(u => u.Role).WithOne().OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
