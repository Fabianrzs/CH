using Ardalis.ApiEndpoints;
using GA2.Apis.GA2.Endpoints.Integraciones;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Dto;
using GA2.Domain.Core.ReferenciasSvc;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using GA2.Application.Dto.TusDatos;
using DocumentFormat.OpenXml.Wordprocessing;

namespace GA2.Apis.GA2.Endpoints.VigiaEndpoints
{
    /// <summary>
    /// Endpoint genera validacion de riesgo en tus datos
    /// </summary>
    [Authorize]
    public class VerificacionRiesgo : BaseAsyncEndpoint.WithRequest<VerificacionRiesgoRequest>.WithResponse<Response<DatosClienteResponse>>
    {

        private readonly ITusDatosClient _client;
        private readonly ILogger<TusDatosController> _logger;

        public VerificacionRiesgo(
           ITusDatosClient client,
           ILogger<TusDatosController> logger)
        {
            _client = client;
            _logger = logger;
        }


        /// <summary>
        /// Consulta información de riesgo del cliente en TusDatos.
        /// </summary>
        /// <returns></returns>
        [HttpGet(
            "api/terceros/validacion/riesgos/tusdatos")]
        [ProducesResponseType(typeof(DatosClienteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async override Task<ActionResult<Response<DatosClienteResponse>>> HandleAsync([FromQuery] VerificacionRiesgoRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
               "Consultando TusDatos para identificación {Identification}",
               request.Identificacion);

            var response = await _client.ConsultarAsync(
                request.Identificacion,
                request.DocumentType,
                request.CheckHistory,
                cancellationToken);

            return Ok(response);
        }
    }
}
