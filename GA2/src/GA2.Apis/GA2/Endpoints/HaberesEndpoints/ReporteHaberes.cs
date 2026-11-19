using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Haberes;
using GA2.Application.Dto.Reintegros;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Haberes;
using GA2.Transversals.Commons;
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
    public class ReporteHaberes : BaseAsyncEndpoint
        .WithRequest<ReportHaberesRequestDto>
        .WithResponse<Response<FileResult>>

    {
        private readonly IHaberesBusinessLogic _haberesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="haberesBusinessLogic"></param>
        public ReporteHaberes(IHaberesBusinessLogic haberesBusinessLogic)
        {
            _haberesBusinessLogic = haberesBusinessLogic;
        }

        /// <summary>
        /// Endpoint para iniciar el pago o desembolso de Reintegros en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/ReporteHaberes")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener informacion Haberes",
            Description = "Obtener informacion Haberes",
            OperationId = "Haberes.ReporteHaberes",
            Tags = new[] { "Haberes" })]
        public async override Task<ActionResult<Response<FileResult>>> HandleAsync(ReportHaberesRequestDto request, CancellationToken cancellationToken = default)
        {
            return await this._haberesBusinessLogic.ReporteHaberes(request);
        }
    }
}
