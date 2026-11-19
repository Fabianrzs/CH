using HangFire;
using System.Threading.Tasks;

namespace Hangfire
{
    public class JobRefresherService
    {
        private readonly HangfireJobsInitializer _initializer;

        public JobRefresherService(HangfireJobsInitializer initializer)
        {
            _initializer = initializer;
        }

        public async Task RefreshJobsAsync()
        {
            await _initializer.RegistrarOActualizarJobsAsync();
        }
    }
}
