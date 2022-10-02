namespace Application.UnitTests.Users.Commands
{
    public class RegisterUserCommandTests : TestBase
    {

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowException_WhenFirstNameIsNullOrEmpty(string firstName)
        {
            var command = new RegisterUserCommand { FirstName = firstName, Email = "email@test.com", Password = "password" };
            await Awaiting(() => SendAsync(command))
                            .Should().ThrowAsync<ValidationException>();
        }


        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowException_WhenLastNameIsNullOrEmpty(string lastName)
        {
            var command = new RegisterUserCommand { FirstName = "firstName", LastName = lastName, Email = "email@test.com", Password = "password" };
            await Awaiting(() => SendAsync(command))
                            .Should().ThrowAsync<ValidationException>();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowException_WhenEmailIsNullOrEmpty(string email)
        {
            var command = new RegisterUserCommand { FirstName = "firstName", LastName = "lastName", Email = email, Password = "password" };
            await Awaiting(() => SendAsync(command))
                            .Should().ThrowAsync<ValidationException>();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task ShouldThrowException_WhenPasswordIsNullOrEmpty(string password)
        {
            var command = new RegisterUserCommand { FirstName = "firstName", LastName = "lastName", Email = "email@test.com", Password = password };
            await Awaiting(() => SendAsync(command))
                            .Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldRaiseUserCreationException_WhenIdentityServiceReturnsErrors()
        {
            //Arrange 
            var command = new RegisterUserCommand { FirstName = "firstName", LastName = "lastName", Email = "email@test.com", Password = "password" };

            MockIdentityService
                .Setup(s => s.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => (Result.Failure(new List<string> { "Invalid Password" }), null));

            //Act
            await Awaiting(() => SendAsync(command))
                   .Should().ThrowAsync<UserCreationException>();
        }

        [Test]
        public async Task ShouldRaiseUserAlreadyExistsException_WhenEmailAlreadyExists()
        {
            //Arrange 
            var command = new RegisterUserCommand { FirstName = "firstName", LastName = "lastName", Email = "existemail@test.com", Password = "password" };

            MockIdentityService
                .Setup(s => s.GetUserByEmailAsync(command.Email))
                .ReturnsAsync(() => new User("firstName", "lastName"));

            //Act
            await Awaiting(() => SendAsync(command))
                   .Should().ThrowAsync<UserAlreadyExistsException>();
        }
    }
}
