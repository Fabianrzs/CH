using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint para consultar intervinientes por tipo y número de documento
    /// </summary>
    public class ConsultarIntervinientePorDocumentoAfiliado :
        BaseAsyncEndpoint.WithRequest<ActualizarIntervinientesActorSolicitudDto>
        .WithResponse<Response<List<IntervinientesActorSolicitud>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        private readonly ILogger<ConsultarIntervinientePorDocumentoAfiliado> _logger;

        public ConsultarIntervinientePorDocumentoAfiliado(
            ISolicitudBusinessLogic solicitudBusinessLogic, ILogger<ConsultarIntervinientePorDocumentoAfiliado> logger)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
            _logger = logger;
        }

        [HttpPost("api/solicitud/ConsultarIntervinientePorDocumentoAfiliado")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<List<IntervinientesActorSolicitud>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar intervinientes por documento",
            Description = "Consulta una lista de intervinientes por tipo y número de documento del afiliado",
            OperationId = "Solicitud.ConsultarIntervinientePorDocumento",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<List<IntervinientesActorSolicitud>>>> HandleAsync(
            ActualizarIntervinientesActorSolicitudDto request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _solicitudBusinessLogic
                    .ConsultarIntervinienteActorTipoYNumeroDocumentoAfiliado(
                        request.TipoDocumentoAfiliado,
                        request.NumeroDocumentoAfiliado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ConsultarIntervinienteActorTipoYNumeroDocumentoAfiliado");

                return new Response<List<IntervinientesActorSolicitud>>
                {
                    IsSuccess = false,
                    ReturnMessage = "Error al consultar los intervinientes",
                    Data = new List<IntervinientesActorSolicitud>()
                };
            }
        }
    }
}
