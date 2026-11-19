
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
    public class CierreMensualCarteraSeven : BaseAsyncEndpoint
        .WithRequest<ParamCierreMensualCarteraSevenDto>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public CierreMensualCarteraSeven(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Cierre Mensual Seven 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCuotaResponse</returns>
        [HttpPost("api/credito/CierreMensualCarteraSeven")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Cierre mensual de los diferentes abonos en Seven",
           Description = "Cierre Mensual en seven",
           OperationId = "Cartera.CierreMensualCarteraSeven",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(ParamCierreMensualCarteraSevenDto request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.CierreMensualCarteraSeven(request, cancellationToken);
        }
    }
}
