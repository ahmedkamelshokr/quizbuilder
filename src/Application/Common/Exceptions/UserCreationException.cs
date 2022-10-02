namespace Application.Common.Exceptions
{
    public class UserCreationException : Exception
    {
        public UserCreationException(string[] errors) : base(string.Join(",", errors))
        {
        }
    }
}
