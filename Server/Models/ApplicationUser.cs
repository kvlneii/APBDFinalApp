using Microsoft.AspNetCore.Identity;

namespace APBDBlazorApp.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual IEnumerable<UsersCompanies> UsersCompanies { get; set; }
    }
}