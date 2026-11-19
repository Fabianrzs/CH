using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GA2.Apis.Identity.Extensions
{
    /// <summary>
    /// HealthChecksExtension verify status api
    /// </summary>
    public static class HealthChecksExtensions
    {
        /// <summary>
        /// Service inject 
        /// </summary>
        /// <param name="services">collection service application</param>
        /// <returns>ServiceInjection health checks</returns>
        public static IServiceCollection AddHealtChecks(this IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            return services;
        }
    }
}
