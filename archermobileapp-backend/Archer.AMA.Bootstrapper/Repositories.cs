using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Archer.AMA.Bootstrapper.Base;
using ArcherMobilApp.DAL.MsSql;
using Archer.AMA.DAL.Contract;

namespace Archer.AMA.Bootstrapper
{
    class Repositories : IDependencyConfigurator
    {
        void IDependencyConfigurator.Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDocumentRepository, DocumentsRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJobOpportunityRepository, JobOpportunityRepository>();
        }
    }
}
