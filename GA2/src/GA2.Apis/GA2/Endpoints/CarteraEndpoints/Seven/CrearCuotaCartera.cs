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
using GA2.Application.Dto.Cartera.Comprobantes;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Seven
{
    /// <summary>
    /// Crea los diferentes tipos de abono en Seven.
    /// </summary>
    /// <author>Camilo Brito</author>
    [Authorize]
    public class CrearCuotaCartera : BaseAsyncEndpoint
        .WithRequest<RequestCarteraSeven>
        .WithResponse<Response<ComprobanteCarteraDto>>
    {
        ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public CrearCuotaCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// crear Cuota en seven 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCuotaResponse</returns>
        [HttpPost("api/credito/CrearCuotaCartera")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ComprobanteCarteraDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear los diferentes abonos en Seven",
           Description = "Crear Cuota en seven",
           OperationId = "Cartera.CrearCuotaCartera",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<ComprobanteCarteraDto>>> HandleAsync(RequestCarteraSeven request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.CrearCuotaCartera(request, cancellationToken);
        }
    }
    
}
