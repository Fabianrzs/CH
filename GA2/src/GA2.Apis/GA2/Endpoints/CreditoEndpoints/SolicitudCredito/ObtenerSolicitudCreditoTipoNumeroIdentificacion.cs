using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]

    public class ObtenerSolicitudCreditoTipoNumeroIdentificacion : BaseAsyncEndpoint.WithRequest<DocumentoDTO>.WithResponse<Response<SolicitudCreditoDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ObtenerSolicitudCreditoTipoNumeroIdentificacion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }





        /// <summary>
        ///  Obtiene solicitud de credito por tipo y numero de documento
        /// </summary>
        /// <param name = "request" > Peticion usuario</param>
        /// <param name = "cancellationToken" > cancelacion si se desea</param>
        /// <returns>Usuario Creado</returns>
        [HttpGet("api/ObtenerSolicitudCreditoTipoNumeroIdentificacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SolicitudCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = " Obtiene solicitud de credito por tipo y numero de documento",
           Description = " Obtiene solicitud de credito por tipo y numero de documento",
           OperationId = "SolicitudCredito.ObtenerSolicitudCreditoTipoNumeroIdentificacion",
           Tags = new[] { "SolicitudCreditoEndpoint" })]
        public override async Task<ActionResult<Response<SolicitudCreditoDto>>> HandleAsync([FromQuery] DocumentoDTO request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerSolicitudCreditoTipoNumeroIdentificacion(request.tipoDocumentoId, request.numeroDocumento);
        }
    }
}
