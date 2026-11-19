using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]

    public class ActualizaSolicCreditoBasica : BaseAsyncEndpoint.WithRequest<SocSolicitudInfobasicaDto>.WithResponse<Response<SocSolicitudInfobasicaDto>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;

        public ActualizaSolicCreditoBasica(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ActualizaSolicCreditoBasica")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SocSolicitudInfobasicaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza Solicitud de Credito datos Basicos",
           Description = "Actualiza Solicitud de Credito datos Basicos",
           OperationId = "Credito.ActualizaSolicCreditoBasica",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<SocSolicitudInfobasicaDto>>> HandleAsync(SocSolicitudInfobasicaDto request, CancellationToken cancellationToken = default)
        {
            var userId = Guid.NewGuid().ToString();
            try
            {
                userId = User.Claims.Where(c => c.Type == "AppIdentity").FirstOrDefault().Value;
            }
            catch
            {
                userId = Guid.NewGuid().ToString();

            }
            request.CreadoPorId = Guid.Parse(userId);
            return await this._creditoBusinessLogic.ActualizaSolicCreditoBasica(request);
        }
    }
}
