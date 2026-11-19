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

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    [Authorize]

    public class SubsanarActividadTramite : BaseAsyncEndpoint.WithRequest<RequestActividadTramiteDto>.WithResponse<Response<ResponseActividadTramiteDto>>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public SubsanarActividadTramite(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/SubsanarActividadTramite")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ResponseActividadTramiteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Subsana actividad tramite",
           Description = "Subsana actividad tramite",
           OperationId = "Credito.SubsanarActividadTramite",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ResponseActividadTramiteDto>>> HandleAsync(RequestActividadTramiteDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.SubsanarActividadTramite(request);       
        }
    }
}
