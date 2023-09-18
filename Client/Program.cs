using APBDBlazorApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

namespace APBDBlazorApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqVEZrXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRcQlRiTn9Wd0BnXXpbeXY=;Mgo+DSMBPh8sVXJ1S0R+XVFPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXdScURmWHpadHFSQ2I=;ORg4AjUWIQA/Gnt2VFhiQlBEfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5ad0JiXH5bdHBQT2Be;MjcwNDQ2MkAzMjMxMmUzMDJlMzBhRWNLcVJDRnh5UGRyakFkR1hFYW5CcCtGR2JiQ1F2M2xRVmVucGtwOW5NPQ==;MjcwNDQ2M0AzMjMxMmUzMDJlMzBqcXhqaUVYeUJTR2ZjNmJ5VEd3eWVWWFo2VmtpSDA5WnF4OUxKa0NMbUQwPQ==;NRAiBiAaIQQuGjN/V0d+Xk9FdlRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31Tf0RkWHpfdXBRQGZfUw==;MjcwNDQ2NUAzMjMxMmUzMDJlMzBYMTNqRmlsUnBZRGN2Tkt4OStweStJZlVRSWNkeENDL1huZzRLV3F6T2VnPQ==;MjcwNDQ2NkAzMjMxMmUzMDJlMzBuRWpYSmpsSVFHUVNEQndKb3FMcTVzVTROMFZZN1o2SlM0VkM2S0JNbWs0PQ==;Mgo+DSMBMAY9C3t2VFhiQlBEfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5ad0JiXH5bdHBSRWde;MjcwNDQ2OEAzMjMxMmUzMDJlMzBFb25qT0w0Y0dWQUhZQndDVWhWUyttL1o2L3VSOGhlYzgvcDdjdjVwK0ZFPQ==;MjcwNDQ2OUAzMjMxMmUzMDJlMzBnV0JwRXdzTWREVnFnZXAyZG80OGVBSk1EMWhSMm5tR1FzdWN0dUJJZXVVPQ==;MjcwNDQ3MEAzMjMxMmUzMDJlMzBYMTNqRmlsUnBZRGN2Tkt4OStweStJZlVRSWNkeENDL1huZzRLV3F6T2VnPQ==");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddSyncfusionBlazor();

            builder.Services.AddHttpClient("APBDBlazorApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("APBDBlazorApp.ServerAPI"));

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}