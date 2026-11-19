
using GA2.Domain.Core.Dodo.Field.Services;
using GA2.Domain.Core.Dodo.File.Services;
using GA2.Domain.Core.Dodo.Forms.Services;
using GA2.Domain.Core.Dodo.Notification.Services;
using GA2.Domain.Core.Dodo.Offices.Services;
using GA2.Domain.Core.Dodo.Token.Services;
using GA2.Domain.Core.Dodo.User.Services;
using GA2.Domain.Core.Dodo.Version.Services;
using GA2.Domain.Core.Dodo.Workflow.Services;
using GA2.Domain.Core.ReferenciasSvc;
using GA2.Domain.Core.ReferenciasSvc.IntegracionesConfig;
using GA2.Infraestructure.Repositories.Integraciones;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using static GA2.Transversals.Commons.Configuraciones.Providers.Dodo;
using static global::GA2.Transversals.Commons.Configuraciones.Providers.TusDatos;

namespace GA2.Apis.GA2.Extensions
{
    public static class HttpClientExtensions
    {

        public static IServiceCollection AddServicesIntegration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<
                IEstadoIntegracionProvider,
                EstadoIntegracionRepository>();

            services.AddTusDatosIntegration(configuration);
            services.AddDodoIntegration(configuration);
            return services;
        }

        public static IServiceCollection AddTusDatosIntegration(
             this IServiceCollection services,
             IConfiguration configuration)
        {
            services.Configure<TusDatosSettings>(
                configuration.GetSection("Integrationes:TusDatos"));

            var settings = configuration
                .GetSection("Integrationes:TusDatos")
                .Get<TusDatosSettings>();

            services.AddHttpClient<
                ITusDatosClient,
                TusDatosClient>(client =>
                {
                    client.BaseAddress = new Uri(settings.BaseUrl);

                    client.Timeout = TimeSpan.FromSeconds(
                        settings.TimeoutSeconds);

                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(
                            "application/json"));

                    client.DefaultRequestHeaders.Add(
                        "x-api-key",
                        settings.ApiKey);
                });

            return services;
        }
        public static IServiceCollection AddDodoIntegration(
               this IServiceCollection services,
               IConfiguration configuration)
        {
            // Bind settings
            services.Configure<DodoSettings>(
                configuration.GetSection("Integrationes:Dodo"));

            var settings = configuration
                .GetSection("Integrationes:Dodo")
                .Get<DodoSettings>();

            // Factory local para no repetir la configuración del HttpClient
            void ConfigureClient(HttpClient client)
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            services.AddHttpClient<IDodoVersionService, DodoVersionService>(ConfigureClient);
            services.AddHttpClient<IDodoTokenService, DodoTokenService>(ConfigureClient);
            services.AddHttpClient<IDodoUserService, DodoUserService>(ConfigureClient);
            services.AddHttpClient<IDodoOfficesService, DodoOfficesService>(ConfigureClient);
            services.AddHttpClient<IDodoNotificationService, DodoNotificationService>(ConfigureClient);
            services.AddHttpClient<IDodoFieldService, DodoFieldService>(ConfigureClient);
            services.AddHttpClient<IDodoFileService, DodoFileService>(ConfigureClient);
            services.AddHttpClient<IDodoFormsService, DodoFormsService>(ConfigureClient);
            services.AddHttpClient<IDodoWorkflowService, DodoWorkflowService>(ConfigureClient);

            return services;
        }
    }
}

