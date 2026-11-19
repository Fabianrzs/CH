using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Seven
{
    /// <summary>
    /// Adelantar cuotas Credito en seven.
    /// </summary>
    /// <author>Camilo Brito</author>
    [Authorize]
    public class AdelantoCuotaCartera : BaseAsyncEndpoint
        .WithRequest<RequestCarteraAdelantoCuotaSeven>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public AdelantoCuotaCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// crear Cuota en seven 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCuotaResponse</returns>
        [HttpPost("api/credito/AdelantoCuotaCartera")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Adelanta los diferentes abonos en Seven",
           Description = "Adelantar Cuota en seven",
           OperationId = "Cartera.AdelantoCuotaCartera",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(RequestCarteraAdelantoCuotaSeven request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.AdelantoCuotaCartera(request, cancellationToken);
        }
    }
}
