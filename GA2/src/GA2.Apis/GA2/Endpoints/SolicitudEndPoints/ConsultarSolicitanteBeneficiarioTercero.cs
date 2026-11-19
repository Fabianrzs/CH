using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Clase del endpoint para consultar por nombres y apellidos los beneficiantes terceros
    /// </summary>
    [Authorize]
    public class ConsultarSolicitanteBeneficiarioTercero :
         BaseAsyncEndpoint.WithRequest<SolicitanteBeneficiarioTerceroSolicitudDto>
        .WithResponse<Response<IEnumerable<SolicitanteBeneficiarioTerceroSolicitud>>>
    {


        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para consultar por nombres y apellidos los beneficiantes terceros
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public ConsultarSolicitanteBeneficiarioTercero(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para consultar por nombres y apellidos los beneficiantes terceros
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/ConsultarSolicitanteBeneficiarioTerceroSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<SolicitanteBeneficiarioTerceroSolicitud>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar beneficiantes terceros por nombre o apellido",
           Description = "Consultar beneficiantes terceros por nombre o apellido",
           OperationId = "Solicitud.ConsultarSolicitanteBeneficiarioTerceroSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<SolicitanteBeneficiarioTerceroSolicitud>>>> HandleAsync(SolicitanteBeneficiarioTerceroSolicitudDto request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarSolicitanteBeneficiarioTerceroSolicitud(request); 
        }
    }
}
