using GA2.Domain.Core.Dodo.Notification.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Notification.Services
{
    public class DodoNotificationService : CommonIntegracionCliente, IDodoNotificationService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoNotificationService(
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoNotificationService> logger)
            : base(httpClient, integracionProvider, auditoriaRepository, logger)
        {
        }

        public Task<Response<SendEmailResponse>> SendEmailAsync(
            SendEmailRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<SendEmailRequest, SendEmailResponse>(
                "api/Notification/SendEmail",
                request,
                cancellationToken);
    }
}
