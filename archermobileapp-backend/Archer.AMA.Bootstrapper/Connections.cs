using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Archer.AMA.Bootstrapper.Base;
using Archer.AMA.DAL.EntityFramework.Base;

namespace Archer.AMA.Bootstrapper
{
    class Connections : IDependencyConfigurator
    {
        void IDependencyConfigurator.Configure(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ArcherContext>(options => options.UseSqlServer(connection));


            //SqlServerTypeMappingSource source = new SqlServerTypeMappingSource()
        }
    }
}
