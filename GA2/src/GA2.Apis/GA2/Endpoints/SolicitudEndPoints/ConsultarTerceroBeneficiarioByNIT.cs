using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint Consultar beneficiario por NIT
    /// </summary>
    public class ConsultarTerceroBeneficiarioByNIT :
         BaseAsyncEndpoint.WithRequest<string>
        .WithResponse<Response<TerceroBeneficiarioRespuestaDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Consultar beneficiario por NIT
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        public ConsultarTerceroBeneficiarioByNIT(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/ConsultarTerceroBeneficiarioByNIT")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerTramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Buscar terceros por NIT",
           Description = "Busca los terceros por NIT",
           OperationId = "Solicitud.ConsultarTerceroBeneficiarioByNIT",
           Tags = new[] { "SolicitudEndpoint" })]
        public override Task<ActionResult<Response<TerceroBeneficiarioRespuestaDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
    
}
