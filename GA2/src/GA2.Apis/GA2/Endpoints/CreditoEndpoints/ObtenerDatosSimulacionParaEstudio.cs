using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
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

    public class ObtenerDatosSimulacionParaEstudio : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<ResponseDatosSimulacionParaEstudioDto>>
    {

        ICreditoBusinessLogic _creditoBusinessLogic;

        public ObtenerDatosSimulacionParaEstudio(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerDatosSimulacionParaEstudio")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ResponseDatosSimulacionParaEstudioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Datos Simulacion Para Estudio",
           Description = "Obtener Datos Simulacion Para Estudio",
           OperationId = "credito.ObtenerDatosSimulacionParaEstudio",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ResponseDatosSimulacionParaEstudioDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerDatosSimulacionParaEstudio(request);
           
        }
    }
}
