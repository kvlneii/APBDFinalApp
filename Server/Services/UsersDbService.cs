using APBDBlazorApp.Server.Data;
using APBDBlazorApp.Server.Models;
using APBDBlazorApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace APBDBlazorApp.Server.Services
{
    public class UsersDbService : IUsersDbService
    {
        private readonly ApplicationDbContext _appDbContext;
        public UsersDbService(ApplicationDbContext context)
        {
            _appDbContext = context;
        }

        public async Task AddCompanyToWatchListAsync(string userId, string companyId)
        {
            var ifCompanyExists = await _appDbContext.UsersCompanies.FirstOrDefaultAsync(e => e.IdCompany == companyId && e.IdUser == userId);
            if(ifCompanyExists == null) 
            {
                await _appDbContext.UsersCompanies.AddAsync(new UsersCompanies
                {
                    IdCompany = companyId,
                    IdUser = userId
                });
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteCompanyFromWatchListAsync(string companyId, string userId)
        {
            var tmp = await _appDbContext.UsersCompanies.FirstOrDefaultAsync(e => e.IdCompany == companyId && e.IdUser == userId);
            _appDbContext.Remove(tmp);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<CompanyDetails>> GetUsersWatchListAsync(string userId)
        {
            return await _appDbContext.UsersCompanies.Where(e => e.IdUser == userId).Include(e => e.Company).Select(e => new CompanyDetails
            {
                IdCompany = e.Company.IdCompany,
                Name = e.Company.Name,
                Logo = e.Company.Logo,
                Website = e.Company.Website,
                Description = e.Company.Description
            }).ToListAsync();
        }
    }
}