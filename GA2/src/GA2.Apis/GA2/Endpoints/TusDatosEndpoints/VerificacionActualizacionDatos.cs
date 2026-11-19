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
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Models;

namespace GA2.Apis.GA2.Endpoints.VigiaEndpoints
{
    /// <summary>
    /// Endpoint genera validacion de riesgo en tus datos
    /// </summary>
    [Authorize]
    public class VerificacionActualizacionDatos : BaseAsyncEndpoint.WithRequest<VerificacionActualizacionRequest>.WithResponse<Response<VigiaResponse>>
    {

        private readonly ITusDatosClient _client;
        private readonly ILogger<TusDatosController> _logger;

        public VerificacionActualizacionDatos(
           ITusDatosClient client,
           ILogger<TusDatosController> logger)
        {
            _client = client;
            _logger = logger;
        }


        /// <summary>
        /// Consulta información de actualizaciond de datos del cliente en TusDatos.
        /// </summary>
        /// <returns></returns>
        [HttpGet(
            "api/terceros/validacion/actualizacion/vigia")]
        [ProducesResponseType(typeof(DatosClienteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async override Task<ActionResult<Response<VigiaResponse>>> HandleAsync([FromQuery] VerificacionActualizacionRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
               "Consultando TusDatos para identificación {Identification}",
               request.Identificacion);

            var response = await _client.ConsultarVigiaAsync(
                request.Identificacion,
                request.DocumentType,
                cancellationToken);

            return Ok(response);
        }

    }
}
