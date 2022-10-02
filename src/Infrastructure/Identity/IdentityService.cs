using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
        }

        public async Task<User> AuthenticateUserAsync(string email, string password, CancellationToken cancellationToken)
        {

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser == null)
                return null;

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, password);

            return isCorrect ? existingUser.User : null;

        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }


        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.User.Id == userId);
            if (user == null)
                return false;
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<(Result Result, string userId)> CreateUserAsync(User user, string password, string email)
        {
            var appUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                User = user
            };

            var result = await _userManager.CreateAsync(appUser, password);

            return (result.ToApplicationResult(), appUser.Id);
        }


        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            return existingUser == null ? null : existingUser.User;
        }

    }
}