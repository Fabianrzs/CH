using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Authorize]
    public class EliminarProcesoAplicacionPagos : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<bool>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public EliminarProcesoAplicacionPagos(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idProceso"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("api/credito/EliminarProcesoAplicacionPagos")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Elimina Procesos de Aplicacion Pagos",
            Description = "Elimina Procesos de Aplicacion Pagos",
            OperationId = "credito.EliminarProcesoAplicacionPagos",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<bool>>> HandleAsync([FromQuery] int idProceso, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.EliminarProcesoAplicacionPagos(idProceso);
        }
    }
}
