using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Worker.Extensions;

namespace Worker
{
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
            services.AddControllersWithViews();
            services.AddHealthChecks();
        }

        /// <summary>
        /// Metodo que acciona el uso de los servicios configurados en la aplicacion
        /// </summary>
        /// <param name="app">Constructor de aplicacion</param>
        /// <param name="env">Entorno en ejecucion</param>
        /// <param name="antiforgery">Antiforgerytoken prevention application</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>19/02/2021</date>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.EnvironmentName.Equals("Development"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorValidation();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
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
            });
        }
    }
}
