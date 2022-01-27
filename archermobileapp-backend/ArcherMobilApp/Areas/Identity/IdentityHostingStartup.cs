using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ArcherMobilApp.Areas.Identity.IdentityHostingStartup))]
namespace ArcherMobilApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}