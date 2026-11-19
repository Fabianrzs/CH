using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Globalization;

namespace GA2.Apis.GA2
{
    /// <summary>
    /// Progrma GA2 API
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Metodo de entrada
        /// </summary>
        /// <param name="args"></param>
        /// <author>Oscar Julian Rojas Garces</author>
        /// <date>07/03/2021</date>
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentCulture;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(GetConfiguration())
                .CreateLogger();

            try
            {
                Log.Information("Iniciando servicio API GA2...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error crítico al iniciar el API");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
        }
        /// <summary>
        /// Create host application
        /// </summary>
        /// <param name="args"></param>
        /// <author>Oscar Julian Rojas Garces</author>
        /// <date>08/03/2021</date>
        /// <returns>Host services</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
