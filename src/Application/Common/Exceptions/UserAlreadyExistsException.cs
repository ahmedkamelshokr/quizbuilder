namespace Application.Common.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string accessCode) : base($"Email {accessCode} already exists.")
        {

        }
    }
}
