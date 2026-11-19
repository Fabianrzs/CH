using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.Afiliaciones
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ReportesInformacionCliente : BaseAsyncEndpoint
        .WithRequest<DocumentoDTO>
        .WithResponse<Response<IEnumerable<InformacionClienteDto>>>
    {
        private readonly IAfiliacionesBusinessLogic _afiliacionesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afiliacionesBusinessLogic"></param>
        public ReportesInformacionCliente(IAfiliacionesBusinessLogic afiliacionesBusinessLogic)
        {
            _afiliacionesBusinessLogic = afiliacionesBusinessLogic;
        }

        /// <summary>
        /// ReporteCarteraInformacionCliente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/ReporteInformacionCliente")]
        [SwaggerOperation(
           Summary = "Reporte Informacion Cliente",
           Description = "Reporte Informacion Cliente",
           OperationId = "afiliaciones.ReporteInformacionCliente",
           Tags = new[] { "AfiliacionesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<InformacionClienteDto>>>> HandleAsync([FromQuery] DocumentoDTO request, CancellationToken cancellationToken = default)
        {
            return await _afiliacionesBusinessLogic.ReportesInformacionCliente(request.numeroDocumento);
        }
    }
}
