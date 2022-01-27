using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AutoMapper;

using Archer.AMA.Bootstrapper.Base;


namespace Archer.AMA.Bootstrapper
{
    public class Bootstraper
    {
        private Bootstraper()
        {

        }

        public static void Init(IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Mappings());
            }).CreateMapper());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSingleton<IDependencyConfigurator, Connections>();
            services.AddSingleton<IDependencyConfigurator, Repositories>();
            services.AddSingleton<IDependencyConfigurator, Services>();
            services.AddSingleton<IDependencyConfigurator, InvalidModelConfigurator>();
            
            services.AddSingleton(configuration);
            var serviceProvider = services.BuildServiceProvider();
            var items = serviceProvider.GetServices<IDependencyConfigurator>();
            foreach (var item in items)
                item.Configure(services, configuration);
        }
    }
}
