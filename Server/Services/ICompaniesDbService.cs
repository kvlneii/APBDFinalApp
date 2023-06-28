using APBDBlazorApp.Server.Models;
using APBDBlazorApp.Shared.Models;

namespace APBDBlazorApp.Server.Services
{
    public interface ICompaniesDbService
    {
        public Task<CompanyDetails> GetCompanyDetailsAsync(string ticker);

        public Task<List<CompanyName>> GetCompaniesByNameAsync(string ticker);

        public Task<StockChartsGet> GetStockChartsAsync(string ticker, string start, string end);

        public Task<StockChartsGet> GetDailyStockChartsAsync(string ticker);

    }
}
