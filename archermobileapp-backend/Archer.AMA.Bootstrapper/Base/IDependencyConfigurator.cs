using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Archer.AMA.Bootstrapper.Base
{
    /// <summary>
    /// Describes Dependency configuration for bootstraper.
    /// </summary>
    interface IDependencyConfigurator
    {
        /// <summary>
        /// Configure dependencies.
        /// </summary>
        /// <param name="services"></param>
        void Configure(IServiceCollection services, IConfiguration configuration);
    }
}
