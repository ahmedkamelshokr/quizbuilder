namespace Application.Common.Exceptions
{
    public class InvalidPayloadException : Exception
    {
        public InvalidPayloadException() : base("Model is not valid!")
        {

        }
        public InvalidPayloadException(List<string> errors) : base("Model is not valid!")
        {
            Errors = errors;
        }
        public InvalidPayloadException(string error) : base("Model is not valid!")
        {
            Errors.Add(error);
        }
        public List<string> Errors { get; set; } = new List<string>();
    }
}

