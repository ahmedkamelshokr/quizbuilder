namespace Application.Common.Models
{
    public record TokenModel
    {
        public string TokenType { get; }
        public string AccessToken { get; }
        public DateTime ExpiresAt { get; }

        public TokenModel(string tokenType, string accessToken, DateTime expiresAt)
            => (TokenType, AccessToken, ExpiresAt) = (tokenType, accessToken, expiresAt);

    }
}
