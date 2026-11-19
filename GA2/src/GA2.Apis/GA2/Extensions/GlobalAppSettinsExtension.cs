using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GA2.Apis.GA2.Extensions
{
    /// <summary>
    /// AddGlobalSettinsOptions
    /// </summary>
    public static class GlobalAppSettinsExtension
    {
        /// <summary>
        /// Extension que inyecta las opciones para aplicacion
        /// </summary>
        /// <param name="services">Instacia de servicios de aplicacion</param>
        /// <param name="configuration">Configuracion de aplicacion</param>
        /// <returns></returns>
        internal static IServiceCollection AddGlobalSettinsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            IConfiguration sectionEmailOptions = configuration.GetSection("Email");
            IConfiguration sectionAppMainOptions = configuration.GetSection("AppMain");
            IConfiguration CHKeysOptions = configuration.GetSection("CHKeys");
            IConfiguration ApisOptions = configuration.GetSection("Apis");
            IConfiguration JwtOptionsOptions = configuration.GetSection("JwtOptions");
            IConfiguration IdVisionOptions = configuration.GetSection("IdVision");
            IConfiguration AqmOptions = configuration.GetSection("Aqm");
            IConfiguration BlobStorageOptions = configuration.GetSection("BlobStorage");
            IConfiguration ConfigsQueueOptions = configuration.GetSection("ConfigsQueue");
            IConfiguration ConfigurationSvcSoa = configuration.GetSection("SvcUtils");
            IConfiguration LocalBlogStorageConfig = configuration.GetSection("CarpetaBlogStorage");

            services.Configure<EmailOptions>(sectionEmailOptions);
            services.Configure<AppMainOptions>(sectionAppMainOptions);
            services.Configure<CHKeysOptions>(CHKeysOptions);
            services.Configure<ApisOptions>(ApisOptions);
            services.Configure<JwtOptions>(JwtOptionsOptions);
            services.Configure<IdVisionOptions>(IdVisionOptions);
            services.Configure<AqmOptions>(AqmOptions);
            services.Configure<BlobStorageOptions>(BlobStorageOptions);
            services.Configure<ConfiguracionesSvc>(ConfigurationSvcSoa);
            services.Configure<LocalBlogStorageConfig>(LocalBlogStorageConfig);

            return services;
        }

        /// <summary>
        /// Extension que inyecta las opciones para aplicacion
        /// </summary>
        /// <param name="services">Instacia de servicios de aplicacion</param>
        /// <param name="configuration">Configuracion de aplicacion</param>
        /// <returns></returns>
        internal static IServiceCollection AddGlobalSettinsOptionsSOA(this IServiceCollection services, IConfiguration configuration)
        {
            IConfiguration sectionSvcConfiguraciones = configuration.GetSection("SvcUtils");
            services.Configure<ConfiguracionesSvc>(sectionSvcConfiguraciones);

            return services;
        }
    }
}
