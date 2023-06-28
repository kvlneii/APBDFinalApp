using APBDBlazorApp.Server.Services;
using APBDBlazorApp.Server.Models;
using APBDBlazorApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APBDBlazorApp.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesDbService _companiesDbService;

        public CompaniesController(ICompaniesDbService service)
        {
            _companiesDbService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompaniesByName(string ticker)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _companiesDbService.GetCompaniesByNameAsync(ticker));
        }

        [HttpGet("{ticker}")]
        public async Task<IActionResult> GetCompanyDetails(string ticker)
        {
            ticker = ticker.ToUpper();

            return Ok(await _companiesDbService.GetCompanyDetailsAsync(ticker));
        }

        [HttpGet("{ticker}/daily")]
        public async Task<IActionResult> GetDailyStockCharts(string ticker)
        {
            return Ok(await _companiesDbService.GetDailyStockChartsAsync(ticker));
        }

        [HttpGet("{ticker}/{from}/{to}")]
        public async Task<IActionResult> GetStocksCharts(string ticker, string from, string to)
        {
            return Ok(await _companiesDbService.GetStockChartsAsync(ticker, from, to));
        }
    }
}
