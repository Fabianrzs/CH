using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Dto;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Models;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static GA2.Transversals.Commons.Configuraciones.Providers.TusDatos;

namespace GA2.Domain.Core.ReferenciasSvc
{
    public class TusDatosClient : CommonIntegracionCliente, ITusDatosClient
    {
        private readonly TusDatosSettings _settings;

        public TusDatosClient(
            HttpClient httpClient,
            IOptions<TusDatosSettings> options,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<TusDatosClient> logger)
            : base(
                  httpClient,
                  integracionProvider,
                  auditoriaRepository,
                  logger)
        {
            _settings = options.Value;
        }

        protected override ReferenciaSvc ReferenciaServicio =>
            ReferenciaSvc.TusDatos;

        public async Task<Response<DatosClienteResponse>> ConsultarAsync(
            string identification,
            string documentType,
            bool checkHistory,
            CancellationToken cancellationToken)
        {
            var endpoint = BuildEndpoint(
                _settings.Endpoints.ConsultarCliente,
                new Dictionary<string, string>
                {
                    { "identification", identification },
                    { "documentType", documentType },
                    { "checkHistory", checkHistory.ToString().ToLower() }
                });

            return await GetAsync<DatosClienteResponse>(
                endpoint,
                cancellationToken);
        }

        public async Task<Response<DatosClienteJobResponse>> ConsultarJobAsync(
            string jobId,
            string identification,
            CancellationToken cancellationToken)
        {
            var endpoint = BuildEndpoint(
                _settings.Endpoints.ConsultarJob,
                new Dictionary<string, string>
                {
                    { "jobId", jobId },
                    { "identification", identification }
                });

            return await GetAsync<DatosClienteJobResponse>(
                endpoint,
                cancellationToken);
        }

        public async Task<Response<VigiaResponse>> ConsultarVigiaAsync(
            string identification,
            string documentType,
            CancellationToken cancellationToken)
                {
                    var endpoint = BuildEndpoint(
                        _settings.Endpoints.ConsultarVigia,
                        new Dictionary<string, string>
                        {
                    { "identification", identification },
                    { "documentType", documentType }
                        });

                    endpoint = endpoint.Replace(
                        $"{_settings.Version}/{_settings.Controller}",
                        _settings.Version);

                    return await GetAsync<VigiaResponse>(
                        endpoint,
                        cancellationToken);
        }

        private string BuildEndpoint(
            string endpoint,
            Dictionary<string, string> values)
        {
            foreach (var value in values)
            {
                endpoint = endpoint.Replace(
                    $"{{{value.Key}}}",
                    value.Value);
            }

            return $"{_settings.Version}/{_settings.Controller}/{endpoint}";
        }
    }
}