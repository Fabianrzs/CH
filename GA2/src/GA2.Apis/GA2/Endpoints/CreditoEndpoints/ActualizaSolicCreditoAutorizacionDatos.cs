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

    public class ActualizaSolicCreditoAutorizacionDatos : BaseAsyncEndpoint.WithRequest<SolicitudCreditoDto>.WithResponse<Response<SolicitudCreditoDto>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;

        public ActualizaSolicCreditoAutorizacionDatos(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ActualizaSolicCreditoAutorizacionDatos")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SolicitudCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza Solicitud de Credito autorizacion de datos",
           Description = "Actualiza Solicitud de Credito autorizacion de datos",
           OperationId = "Credito.ActualizaSolicCreditoAutorizacionDatos",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<SolicitudCreditoDto>>> HandleAsync(SolicitudCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ActualizaSolicCreditoAutorizacionDatos(request); 
        }
    }
}
