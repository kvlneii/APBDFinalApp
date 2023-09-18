using APBDBlazorApp.Server.Data;
using APBDBlazorApp.Server.Models;
using APBDBlazorApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace APBDBlazorApp.Server.Services
{
    public class CompaniesDbService : ICompaniesDbService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public CompaniesDbService(ApplicationDbContext context, IConfiguration configuration)
        {
            _appDbContext = context;
            _configuration = configuration;
        }

        public async Task<List<CompanyName>> GetCompaniesByNameAsync(string ticker)
        {
            List<CompanyName> companyNames = new List<CompanyName>();
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.polygon.io/v3/reference/tickers?search=" + ticker + "&active=true&sort=ticker&order=asc&limit=1000&apiKey=" + _configuration.GetConnectionString("PolygonKey"));
                    var responseBody = await response.Content.ReadAsStringAsync();
                    JsonDocument jsonDocument = JsonDocument.Parse(responseBody);

                    JsonElement companies = jsonDocument.RootElement.GetProperty("results");
                    foreach (var company in companies.EnumerateArray())
                    {
                        try
                        {
                            companyNames.Add(new CompanyName
                            {
                                Ticker = company.GetProperty("ticker").ToString(),
                                Name = company.GetProperty("name").ToString()
                            });
                        }
                        catch (Exception ex) when (ex is InvalidOperationException || ex is KeyNotFoundException) { continue; }
                    }
                }
                catch (Exception)
                {
                    if (ticker != null && await _appDbContext.Companies.AnyAsync(e => e.IdCompany.ToUpper().Contains(ticker.ToUpper())))
                    {
                        companyNames = await _appDbContext.Companies.Where(e => e.IdCompany.ToUpper().Contains(ticker.ToUpper())).Select(e => new CompanyName
                        {
                            Name = e.Name,
                            Ticker = e.IdCompany
                        }).ToListAsync();
                    }
                }
            }
            return companyNames;
        }

        public async Task<CompanyDetails> GetCompanyDetailsAsync(string ticker)
        {
            var companyFromDb = await _appDbContext.Companies.FirstOrDefaultAsync(e => e.IdCompany == ticker);

            if (companyFromDb != null)
            {
                return new CompanyDetails
                {
                    IdCompany = companyFromDb.IdCompany,
                    Name = companyFromDb.Name,
                    Logo = companyFromDb.Logo,
                    Website = companyFromDb.Website,
                    Description = companyFromDb.Description
                };
            }
            else
            {
                CompanyDetails companyDetails = new CompanyDetails();
                using (var client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync("https://api.polygon.io/v3/reference/tickers/" + ticker + "?apiKey=" + _configuration.GetConnectionString("PolygonKey"));
                        var responseBody = await response.Content.ReadAsStringAsync();
                        JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                        JsonElement results = jsonDocument.RootElement.GetProperty("results");
                        string website = "";
                        string logo = "";
                        string description = "";
                        try
                        {
                            website = results.GetProperty("homepage_url").ToString();
                        }
                        catch (Exception)
                        {
                            website = "No website provided";
                        }

                        try
                        {
                            logo = results.GetProperty("branding").GetProperty("logo_url").ToString();
                        }
                        catch (Exception)
                        {
                            logo = "No logo provided";
                        }

                        try
                        {
                            description = results.GetProperty("description").ToString();
                        }
                        catch (Exception)
                        {
                            description = "No description provided";
                        }

                        companyDetails = new CompanyDetails
                        {
                            IdCompany = results.GetProperty("ticker").ToString(),
                            Name = results.GetProperty("name").ToString(),
                            Website = website,
                            Logo = logo,
                            Description = description
                        };

                        if (companyDetails.IdCompany != null)
                        {
                            await _appDbContext.Companies.AddAsync(new Company
                            {
                                IdCompany = companyDetails.IdCompany,
                                Name = companyDetails.Name,
                                Website = companyDetails.Website,
                                Logo = companyDetails.Logo,
                                Description = companyDetails.Description
                            });

                            await _appDbContext.SaveChangesAsync();
                        }
                        
                    }
                    catch (Exception) { }
                }
                return companyDetails;
            }
        }

        public async Task<StockChartsGet> GetStockChartsAsync(string ticker, string start, string end)
        {
            StockChartsGet stocks = new StockChartsGet();
            stocks.StockCharts = new List<StockCharts>();
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.polygon.io/v2/aggs/ticker/" + ticker.ToUpper() + "/range/1/day/" + start +
                        "/" + end + "?adjusted=true&sort=asc&&apiKey=" + _configuration.GetConnectionString("PolygonKey"));

                    var responseBody = await response.Content.ReadAsStringAsync();
                    JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                    stocks.ResultsCount = int.Parse(jsonDocument.RootElement.GetProperty("resultsCount").ToString());

                    if (stocks.ResultsCount != 0)
                    {
                        JsonElement results = jsonDocument.RootElement.GetProperty("results");
                        stocks.StockCharts = new List<StockCharts>();
                        foreach (var chart in results.EnumerateArray())
                        {
                            try
                            {
                                stocks.StockCharts.Add(new StockCharts
                                {
                                    DateTime = DateTimeFromUnixTimestampMillis(long.Parse(chart.GetProperty("t").ToString())),
                                    Open = double.Parse(chart.GetProperty("o").ToString(), CultureInfo.InvariantCulture),
                                    Close = double.Parse(chart.GetProperty("c").ToString(), CultureInfo.InvariantCulture),
                                    High = double.Parse(chart.GetProperty("h").ToString(), CultureInfo.InvariantCulture),
                                    Low = double.Parse(chart.GetProperty("l").ToString(), CultureInfo.InvariantCulture),
                                    Volume = double.Parse(chart.GetProperty("v").ToString(), CultureInfo.InvariantCulture)
                                });
                            }
                            catch (Exception ex) when (ex is InvalidOperationException || ex is KeyNotFoundException) { continue; }
                        }
                    }
                }
                catch (Exception)
                {
                    stocks.ResultsCount = 0;
                }
            }
            return stocks;
        }

        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            DateTime begin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return begin.AddMilliseconds(millis);
        }

        public async Task<StockChartsGet> GetDailyStockChartsAsync(string ticker)
        {
            StockChartsGet stocks = new StockChartsGet();
            stocks.StockCharts = new List<StockCharts>();
            JsonDocument jsonDocument;
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.polygon.io/v2/aggs/ticker/" + ticker.ToUpper() + "/range/30/minute/" + DateTime.UtcNow.Date.AddDays(-2).ToString("yyyy-MM-dd") +
                        "/" + DateTime.UtcNow.Date.AddDays(-1).ToString("yyyy-MM-dd") + "?adjusted=true&sort=asc&&apiKey=" + _configuration.GetConnectionString("PolygonKey"));

                    var responseBody = await response.Content.ReadAsStringAsync();
                    jsonDocument = JsonDocument.Parse(responseBody);

                    if (!await _appDbContext.CompanyDailyTrades.AnyAsync(e => e.IdCompany == ticker && e.DateTime == DateTime.UtcNow.Date))
                    {
                        var charts = new CompanyDailyTrades
                        {
                            DateTime = DateTime.UtcNow.Date,
                            IdCompany = ticker,
                            Json = responseBody
                        };
                        await _appDbContext.CompanyDailyTrades.AddAsync(charts);
                        await _appDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        var charts = await _appDbContext.CompanyDailyTrades.FirstOrDefaultAsync(e => e.IdCompany == ticker && e.DateTime == DateTime.UtcNow.Date);
                        charts.Json = responseBody;
                        await _appDbContext.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    var chartsFromDb = await _appDbContext.CompanyDailyTrades.FirstOrDefaultAsync(e => e.IdCompany == ticker && e.DateTime == DateTime.UtcNow.Date);
                    if (chartsFromDb == null)
                    {
                        return new StockChartsGet
                        {
                            ResultsCount = 0,
                            StockCharts = null
                        };
                    }
                    else
                    {
                        jsonDocument = JsonDocument.Parse(chartsFromDb.Json);
                    }
                }


                JsonElement status = jsonDocument.RootElement.GetProperty("status");
                JsonElement count = jsonDocument.RootElement.GetProperty("resultsCount");
                stocks.ResultsCount = int.Parse(jsonDocument.RootElement.GetProperty("resultsCount").ToString());
                if (stocks.ResultsCount != 0)
                {
                    JsonElement results = jsonDocument.RootElement.GetProperty("results");
                    foreach (var chart in results.EnumerateArray())
                    {
                        try
                        {
                            stocks.StockCharts.Add(new StockCharts
                            {
                                DateTime = DateTimeFromUnixTimestampMillis(long.Parse(chart.GetProperty("t").ToString())),
                                Open = double.Parse(chart.GetProperty("o").ToString(), CultureInfo.InvariantCulture),
                                Close = double.Parse(chart.GetProperty("c").ToString(), CultureInfo.InvariantCulture),
                                High = double.Parse(chart.GetProperty("h").ToString(), CultureInfo.InvariantCulture),
                                Low = double.Parse(chart.GetProperty("l").ToString(), CultureInfo.InvariantCulture),
                                Volume = double.Parse(chart.GetProperty("v").ToString(), CultureInfo.InvariantCulture)
                            });
                        }
                        catch (Exception ex) when (ex is InvalidOperationException || ex is KeyNotFoundException) { continue; }
                    }
                }
            }
            return stocks;
        }

    }
}
