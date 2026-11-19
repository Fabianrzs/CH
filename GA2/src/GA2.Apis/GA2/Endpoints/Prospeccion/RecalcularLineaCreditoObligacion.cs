using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    public class RecalcularLineaCreditoObligacion : BaseAsyncEndpoint.WithRequest<DatosLineaCreditoObligacionDto>.WithResponse<Response<LineaCreditoObligacion>>
    {

        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para Restaura el valor del factor financiero
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public RecalcularLineaCreditoObligacion(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("/api/prospeccion/RestaurarLineasCreditoObligacion")]
        [ProducesResponseType(typeof(Response<LineaCreditoObligacion>), StatusCodes.Status200OK)]
        [SwaggerOperation("Restaura el valor del factor financiero",
            Description = "Restaura el valor del factor financiero",
            OperationId = "prospeccion.RestaurarLineasCreditoObligacion",
            Tags = new[] { "ProspeccionEndpoint" })]
        public override async Task<ActionResult<Response<LineaCreditoObligacion>>> HandleAsync(DatosLineaCreditoObligacionDto request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.LineasCreditoRestaurar(request); 
        }
    }
}
