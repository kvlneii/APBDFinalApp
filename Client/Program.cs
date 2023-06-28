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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqVk1nQ1BCaV1CX2BZeVl2TGlZek4BCV5EYF5SRHBeRV1gS3hTf0BrXHg=;Mgo+DSMBPh8sVXJ1S0R+X1pDaV5EQmFJfFBmQmlZdlRwcUU3HVdTRHRcQltiTn5QdENjUHpXeHM=;ORg4AjUWIQA/Gnt2VFhiQlJPcUBDW3xLflF1VWRTf1d6dFBWESFaRnZdQV1lSHlTdUdlWXZYcXZS;MjQ0NTM2MEAzMjMxMmUzMDJlMzBGRmNpRlYybit0b1JCUXJtcFZTT0thNEpsT1phcWdMNnNNc0JDSjlkWjNRPQ==;MjQ0NTM2MUAzMjMxMmUzMDJlMzBqWW5VTEVRK1ZrZEpmbENhcmdhZGRYZjEzR0VNN0dmZ2JKazdnU0JKendJPQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfVhdXGVWfFN0RnNedV55flVDcC0sT3RfQF5jT39UdkZhX39XdnJQQQ==;MjQ0NTM2M0AzMjMxMmUzMDJlMzBLSnNhQ3IxUTR3T1orejVjb1VpL3A5ZE5qVFBDWlhsK0VTRmJJcnk1OG5NPQ==;MjQ0NTM2NEAzMjMxMmUzMDJlMzBiRUxMdklpd1dUK3MvUzBJRmxjVWdpNUNUWjNWWXlVT0hKVWJWZWtqV3JvPQ==;Mgo+DSMBMAY9C3t2VFhiQlJPcUBDW3xLflF1VWRTf1d6dFBWESFaRnZdQV1lSHlTdUdlWXZZcnFS;MjQ0NTM2NkAzMjMxMmUzMDJlMzBHTVplL3RJQ0JlSmF2TGFNdG5kUmp3eTU2MTVtdTV4Y1kwdDdsQnVIa2Z3PQ==;MjQ0NTM2N0AzMjMxMmUzMDJlMzBZMnhUcGxUZGFVcmFyZkdEa1VRaTJiL0JaQm5kdHkxSXhHWnlqZ1YrQ0g4PQ==;MjQ0NTM2OEAzMjMxMmUzMDJlMzBLSnNhQ3IxUTR3T1orejVjb1VpL3A5ZE5qVFBDWlhsK0VTRmJJcnk1OG5NPQ==");

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