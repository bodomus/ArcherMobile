using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Archer.AMA.Bootstrapper.Base;
using ArcherMobilApp.BLL;
using Archer.AMA.BLL.Contracts;

namespace Archer.AMA.Bootstrapper
{
    class Services : IDependencyConfigurator
    {
        void IDependencyConfigurator.Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJobOpportunityService, JobOpportunityService>();
        }
    }
}
