using APBDBlazorApp.Shared.Models;

namespace APBDBlazorApp.Server.Services
{
    public interface IUsersDbService
    {
        public Task AddCompanyToWatchListAsync(string userId, string companyId);
        public Task<List<CompanyDetails>> GetUsersWatchListAsync(string userId);
        public Task DeleteCompanyFromWatchListAsync(string companyId, string userId);
    }
}
