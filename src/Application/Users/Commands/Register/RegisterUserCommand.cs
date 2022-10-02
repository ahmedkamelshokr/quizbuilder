namespace Application.Users.Commands
{
    public class RegisterUserCommand : IRequest<AuthenticatedUserDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthenticatedUserDto>
    {
        private readonly IApplicationRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public RegisterUserCommandHandler(
            IApplicationRepository repository,
            IIdentityService identityService,
             ITokenService tokenService
        )
        {
            _repository = repository;
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedUserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await Validate(request);

            var user = await CreateUserAsync(request, cancellationToken);

            var userCreation = await _identityService.CreateUserAsync(user, request.Password, request.Email);

            if (!userCreation.Result.Succeeded)
                throw new UserCreationException(userCreation.Result.Errors);

            await _repository.SaveChangesAsync(cancellationToken);

            var token = BuildJwtToken(user);

            return new AuthenticatedUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token.AccessToken
            };
        }

        private async Task Validate(RegisterUserCommand request)
        {
            var existingUser = await _identityService.GetUserByEmailAsync(request.Email);

            if (existingUser != null)
                throw new UserAlreadyExistsException(request.Email);
        }

        private async Task<User> CreateUserAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.FirstName, request.LastName);

            await _repository.Users.AddAsync(user, cancellationToken);
            return user;
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