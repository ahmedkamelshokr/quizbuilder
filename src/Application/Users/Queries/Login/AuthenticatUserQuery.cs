namespace Application.Users.Queries
{
    public class AuthenticateUserQuery : IRequest<AuthenticatedUserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateUserQueryHandler : IRequestHandler<AuthenticateUserQuery, AuthenticatedUserDto>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public AuthenticateUserQueryHandler(
            IIdentityService identityService,
            ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedUserDto> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.AuthenticateUserAsync(request.Email, request.Password, cancellationToken);
            if (user == null)
                throw new UnauthorizedAccessException();


            var token = BuildJwtToken(user);

            return new AuthenticatedUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token.AccessToken
            };
        }

        private TokenModel BuildJwtToken(User user)
        {
            var claims = new List<(string claim, string claimValue)>();
            //add any needed claims
            var token = _tokenService.CreateAuthenticationToken(user.Id, $"{user.FirstName} {user.LastName}", claims);
            return token;
        }
    }
}

