
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolios> Portfolios { get; set; } = new List<Portfolios>();
    }
}
