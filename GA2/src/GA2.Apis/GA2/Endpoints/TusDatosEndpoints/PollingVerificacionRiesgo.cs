using Ardalis.ApiEndpoints;
using GA2.Apis.GA2.Endpoints.Integraciones;
using GA2.Application.Dto;
using GA2.Application.Dto.TusDatos;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Dto;
using GA2.Domain.Core.ReferenciasSvc;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using GA2.Domain.Entities;

namespace GA2.Apis.GA2.Endpoints.VigiaEndpoints
{
    /// <summary>
    /// Endpoint genera polling de validacion de riesgo en tus datos 
    /// </summary>
    [Authorize]
    public class PollingVerificacionRiesgo : BaseAsyncEndpoint.WithRequest<PollingVerificacionRiesgoRequest>.WithResponse<Response<DatosClienteJobResponse>>
    {
        private readonly ITusDatosClient _client;
        private readonly ILogger<TusDatosController> _logger;
  

        public PollingVerificacionRiesgo(
           ITusDatosClient client,
           ILogger<TusDatosController> logger)
        {
            _client = client;
            _logger = logger;
        }



        [HttpGet("api/terceros/validacion/riesgos/tusdatos/polling")]
        [ProducesResponseType(
           typeof(DatosClienteJobResponse),
           StatusCodes.Status200OK)]
        [ProducesResponseType(
           StatusCodes.Status500InternalServerError)]
        public async override Task<ActionResult<Response<DatosClienteJobResponse>>> HandleAsync(PollingVerificacionRiesgoRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                           "Consultando estado job TusDatos {JobId}",
                           request.JobId);

            var response = await _client.ConsultarJobAsync(
                request.JobId,
                request.Identificacion,
                cancellationToken);

            return Ok(response);
        }
    }
}
