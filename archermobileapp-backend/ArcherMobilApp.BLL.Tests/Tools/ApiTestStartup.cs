using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ArcherMobilApp.BLL.Tests.Tools
{
    public class ApiTestStartup : Startup
    {
        public ApiTestStartup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
        {
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*ILoggerFactory loggerFactory;

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/myapp-{Date}.txt", LogLevel.Debug);*/
            app.UseMiddleware<FakeRemoteIpAddressMiddleware>();
            base.Configure(app, env);
        }
    }
}