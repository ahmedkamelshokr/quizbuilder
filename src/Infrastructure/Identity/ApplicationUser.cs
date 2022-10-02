using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser: IdentityUser  {
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
