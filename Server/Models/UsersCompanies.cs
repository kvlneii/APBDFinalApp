using APBDBlazorApp.Shared.Models;

namespace APBDBlazorApp.Server.Models
{
    public class UsersCompanies
    {
        public string IdUser { get; set; }
        public string IdCompany { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Company Company { get; set; }
    }
}
