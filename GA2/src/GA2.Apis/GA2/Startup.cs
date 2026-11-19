using Autofac.Core;
using GA2.Api.Extensiones;
using GA2.Apis.GA2.Extensions;
using GA2.Application.Main;
using GA2.BuildingBlocks.EventBus.Abstractions;
using GA2.Domain.Core.IntegrationEvents.Events.WorkManager;
using GA2.Domain.Core.IntegrationEvents.Integrations.WorkManager;
using GA2.Transversals.Commons.Constants;
using Hangfire;
using Hangfire.MemoryStorage;
using HangFire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace GA2.Apis.GA2
{
    /// <summary>
    /// Clase inicializadora de servicios de applicación
    /// </summary>
    /// <author>Oscar Julian Rojas Garces</author>
    /// <date>19/02/2021</date>
    public class Startup
    {
        /// <summary>
        /// Constructor de inicio de servicios
        /// </summary>
        /// <param name="configuration">Configuración de la aplicacion</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Propiedad que expone el archivo de configuracin de la aplicación 
        /// </summary>
        /// <author>Oscar Julian Rojas </author>
        /// <date>19/02/201</date>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Metodo que expone los servicios configurados en la aplicacion
        /// </summary>
        /// <author>Oscar Julian Rojas Garces</author>
        /// <date>19/02/2021</date>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomCacheApplication(Configuration);
            services.AddGlobalSettinsOptions(Configuration);
            services.AddGlobalSettinsOptionsSOA(Configuration);

            //Extensiones de servicios
            services.AddCorsServices(Configuration);
            services.AddControllersWithViews();

            services.AddAntiforgery(setup =>
            {
                setup.HeaderName = "GA2-XSRF-TOKEN";
            });

            services.AddJwtDocumentation(Configuration);
            services.AddSwaggerDocumentation(Configuration);

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddSignalR();

            // Inject Dependencies
            #region Integraciones
            services.AddServicesIntegration(Configuration);
            #endregion
            services.AddServicesDependencies();
            services.AddHealtChecks();
            services.AddCustomIntegrations(Configuration);
            services.AddEventBus(Configuration);

            services.AddHangfire(config => config.UseMemoryStorage());
            services.AddHangfireServer();
            services.AddTransient<AcreditacionHangFireService>();
            services.AddSingleton<HangfireJobsInitializer>();

        }

        /// <summary>
        /// Metodo que acciona el uso de los servicios configurados en la aplicacion
        /// </summary>
        /// <param name="app">Constructor de aplicacion</param>
        /// <param name="env">Entorno en ejecucion</param>
        /// <param name="antiforgery">antiforgery token application</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>19/02/2021</date>
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorValidation();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(GA2Constants.ApiName);
            app.UseSwaggerDocumentation();

            app.UseJwtAuthentication();
            app.UseAuthorization();

            // app.UseAntiforgeryToken(antiforgery);
            app.UseForwardedHeaders(new ForwardedHeadersOptions() { 
                ForwardedHeaders= ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
                endpoints.MapHub<HubNotificaciones>("api/hubs/notificacions");
            });

            ConfigureEventBus(app);

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });


            using (var scope = app.ApplicationServices.CreateScope())
            {
                var jobInitializer = scope.ServiceProvider.GetRequiredService<HangfireJobsInitializer>();
                jobInitializer.RegistrarOActualizarJobsAsync().GetAwaiter().GetResult();

                RecurringJob.AddOrUpdate<JobRefresherService>(
                    "job-refresh",
                    x => x.RefreshJobsAsync(),
                    "*/1 * * * *" // cada 5 minutos verifica si hay cambios en BD
);
            }

        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<WorkManagerInsertRequestIntegrationEvent, WorkManagerInsertRequestIntegrationEventHandler>();
            eventBus.Subscribe<WorkManagerUpdateRequestIntegrationEvent, WorkManagerUpdateRequestIntegrationEventHandler>();
            eventBus.Subscribe<WorkManagerDeleteRequestIntegrationEvent, WorkManagerDeleteRequestIntegrationEventHandler>();
            eventBus.Subscribe<WorkManagerWorkflowAttachFilesRequestIntegrationEvent, WorkManagerWorkflowAttachFilesRequestIntegrationEventHandler>();
            eventBus.Subscribe<WorkManagerAttachRequestIntegrationEvent, WorkManagerAttachRequestIntegrationEventHandler>();
        }
    }
}
