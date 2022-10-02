namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<User> AuthenticateUserAsync(string email, string password, CancellationToken cancellationToken);

        Task<string> GetUserNameAsync(string userId);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result Result, string userId)> CreateUserAsync(User user, string password, string email);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> IsInRoleAsync(string userId, string role);

    }
}