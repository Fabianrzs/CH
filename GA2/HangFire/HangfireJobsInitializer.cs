using GA2.Infraestructure.Repositories;
using Hangfire;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire
{
    public class HangfireJobsInitializer
    {
        private readonly IRecurringJobManager _jobManager;
        private readonly ICatalogosRepository _catalogosRepository;
        private readonly IServiceProvider _serviceProvider;

        public HangfireJobsInitializer(IRecurringJobManager jobManager, ICatalogosRepository catalogosRepository, IServiceProvider serviceProvider)
        {
            _jobManager = jobManager;
            _catalogosRepository = catalogosRepository;
            _serviceProvider = serviceProvider; 
        }


        public async Task RegistrarOActualizarJobsAsync()
        {
            var configuraciones = await _catalogosRepository.ObtenerParametrizacionHangFire();
            var config = configuraciones.FirstOrDefault();
                try
                {
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                    if (config.CVL_ACTIVO == "True")
                    {
                         _jobManager.AddOrUpdate<AcreditacionHangFireService>(
                             config.CVL_CODIGO,
                            job => job.Execute(),
                            config.CVL_DESCRIPCION.Trim(),
                            timeZone
                            );
                    }
                }
                catch (Exception e)
                {
                    await _catalogosRepository.InsertarLogHangFire(e.Message, "Fallo try catch en el inicio"); ;
                }

            
        }
    }
}
