using System.IO;
using System.Reflection;
using ArcherMobilApp.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit.Abstractions;

namespace ArcherMobilApp.BLL.Tests.Controllers.User
{
    public class ExampleAppTestFixture : WebApplicationFactory<Program>
    {
        // Must be set in each test
        public ITestOutputHelper Output { get; set; }
        public string assemblyName;

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();
            return builder
                .ConfigureLogging((logBuilder) =>
                {
                    logBuilder.AddConsole().SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure((ctx, app) =>
                        {
                            if (ctx.HostingEnvironment.IsDevelopment())
                            {
                                app.UseDeveloperExceptionPage();
                            }

                            app.UseAuthorization();
                            app.UseRouting();
                            app.UseHttpsRedirection();
                            app.UseStaticFiles();
                            app.UseEndpoints(endpoints =>
                            {
                                // endpoints.MapRazorPages();
                                // endpoints.MapControllers();
                                // endpoints.MapControllerRoute("default", "/{action}/{id?}", new { action = "Index", controller = "WeatherForecast" });
                                endpoints.MapControllerRoute(
                                    name: "default",
                                    pattern: "{controller=Home}/{action=Index}/{id?}");
                            });
                        })
                        .ConfigureServices((serviceCollection) =>
                        {
                            serviceCollection.AddAuthorization();
                            serviceCollection.AddRazorPages();
                            serviceCollection.AddMvc(op => op.EnableEndpointRouting = true);
                        })
                        .UseSetting(WebHostDefaults.ApplicationKey, assemblyName)
                        .ConfigureAppConfiguration((ctx, configBuilder) =>
                        {
                            configBuilder.Build();
                        });
                });
        }

    

        /*protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Don't run IHostedServices when running as a test
            builder.ConfigureTestServices((services) =>
            {
                services.RemoveAll(typeof(IHostedService));
                services.AddControllers().AddApplicationPart(typeof(UserController).Assembly);
                services.AddControllersWithViews().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                }).AddRazorRuntimeCompilation();
            });
        }*/
    }
}