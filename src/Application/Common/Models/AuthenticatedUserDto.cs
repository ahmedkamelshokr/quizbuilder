namespace Application.Common.Models
{
    public class AuthenticatedUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> RoleNames { get; set; } = new List<string>();

    }
}
