using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Haberes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.HaberesEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class GenerarReporteHaberes: BaseAsyncEndpoint.WithRequest<HaberesRequestDto>.WithResponse<IFormFile>
    {
        private readonly IHaberesBusinessLogic _haberesBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="haberesBusinessLogic"></param>
        public GenerarReporteHaberes(IHaberesBusinessLogic haberesBusinessLogic)
        {
            _haberesBusinessLogic = haberesBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/generarReporteHaberes")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Haberes",
            Description = "Generar Reporte Haberes",
            OperationId = "Haberes.GenerarReporteHaberes",
            Tags = new[] { "Haberes" })]
        
        public async override Task<ActionResult<IFormFile>> HandleAsync(HaberesRequestDto request, CancellationToken cancellationToken = default)
        {
            return await this._haberesBusinessLogic.GenerarReporteHaberes(request);
        }
    }
}
