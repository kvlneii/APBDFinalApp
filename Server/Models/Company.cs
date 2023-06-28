using APBDBlazorApp.Server.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APBDBlazorApp.Shared.Models
{
    public class Company
    {
        public string IdCompany { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public virtual IEnumerable<UsersCompanies> UsersCompanies { get; set; }
        public virtual IEnumerable<CompanyDailyTrades> CompanyDailyTrades { get; set; }
    }
}