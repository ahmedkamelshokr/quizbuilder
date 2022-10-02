global using Application.Common.Interfaces;
global using Application.Common.RepositoryExtensions;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Moq;
global using FluentAssertions;
global using Domain.Entities;
global using static FluentAssertions.FluentActions;
global using Domain.Exceptions;
global using Application.Common.Exceptions;
global using Application.Quizs.Commands;
global using Application.Users.Commands;
global using Application.Common.Models;
using MediatR;

namespace Application.UnitTests
{
    public class TestBase
    {

        protected readonly IApplicationRepository Repository;
        protected static Mock<ICurrentUserService> MockCurrentUserService;
        protected static Mock<IIdentityService> MockIdentityService;
        protected static Mock<ITokenService> MockTokenService;

        protected User quizeOwner;
        private static IServiceScopeFactory _scopeFactory;

        public TestBase()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddApplication();
            AddInMemoryRepository(services);
            AddMockedInfrastructure(services);
            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            Repository = FindService<IApplicationRepository>();
        }

        [SetUp]
        public async Task TestSetUp()
        {
            quizeOwner = new User("first name", "last name");
            Repository.Users.Add(quizeOwner);
            await Repository.SaveChangesAsync();

            MockCurrentUserService.SetupGet(x => x.UserId).Returns(quizeOwner.Id);

        }

        private static void AddInMemoryRepository(IServiceCollection services)
        {
            services.AddDbContext<InMemoryRepository>(o => o.UseInMemoryDatabase("QuizBuilderDB"));
            services.AddSingleton<IApplicationRepository>(provider => provider.GetService<InMemoryRepository>());
        }


        protected static T FindService<T>() where T : class
        {
            var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }

        protected static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var mediator = FindService<IMediator>();
            return await mediator.Send(request);
        }
        private static void AddMockedInfrastructure(IServiceCollection services)
        {
            var currentUserService = Mock.Of<ICurrentUserService>();
            var identityService = Mock.Of<IIdentityService>();
            var tokenService = Mock.Of<ITokenService>();

            services.AddTransient(_ => currentUserService);
            services.AddTransient(_ => identityService);
            services.AddTransient(_ => tokenService);

            MockCurrentUserService = Mock.Get(currentUserService);
            MockIdentityService = Mock.Get(identityService);
            MockTokenService = Mock.Get(tokenService);
        }
    }
}