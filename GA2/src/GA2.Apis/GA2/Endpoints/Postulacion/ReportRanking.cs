using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ReportRanking : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<IEnumerable<ReportRankingDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bussigneLogic"></param>
    public ReportRanking(IPostulacionBussineLogic bussigneLogic)
    {
        _postulacionBussineLogic = bussigneLogic;
    }
    [HttpGet("api/postulante/GetReportRanking")]
    [ProducesResponseType(typeof(Response<IEnumerable<ReportRankingDto>>), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "obtener los reporte de fondo de solidaridad ",
        Description = "obtener los reporte de ranking de fondo de solidaridad",
        OperationId = "postulante.GetReportRanking",
        Tags = new[] { "PostulanteEndPoint" })]
    public async override Task<ActionResult<Response<IEnumerable<ReportRankingDto>>>> HandleAsync(int idConvocatoria,CancellationToken cancellationToken = default)
    {
            return await _postulacionBussineLogic.GetReporteRanking(idConvocatoria);
    }
}
}
