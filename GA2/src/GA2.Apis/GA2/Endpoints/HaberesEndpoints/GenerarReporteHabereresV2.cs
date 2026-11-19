using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Haberes;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.HaberesEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class GenerarReporteHaberesV2 : BaseAsyncEndpoint.WithRequest<HaberesRequestDto>.WithResponse<IFormFile>
    {
        private readonly IHaberesBusinessLogic _haberesBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="haberesBusinessLogic"></param>
        public GenerarReporteHaberesV2(IHaberesBusinessLogic haberesBusinessLogic)
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
        [HttpPost("api/credito/generarReporteHaberesv2")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Haberes Version 2",
            Description = "Generar Reporte Haberes",
            OperationId = "Haberes.GenerarReporteHaberes",
            Tags = new[] { "Haberes" })]

        public async override Task<ActionResult<IFormFile>> HandleAsync(HaberesRequestDto request, CancellationToken cancellationToken = default)
        {
            return await this._haberesBusinessLogic.GenerarReporteHaberesV2(request);
        }
    }
}
