namespace Domain.Entities
{
    public class User : Entity
    {
        public User(string firstName, string lastName)
        {
            Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
            Id = Guid.NewGuid().ToString();
            FirstName = firstName;
            LastName = lastName;
        }
        [Required][StringLength(50)] public new string Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Quiz> Quizs { get; set; }


    }

}