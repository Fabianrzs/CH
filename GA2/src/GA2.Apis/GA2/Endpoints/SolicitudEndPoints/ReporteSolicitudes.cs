using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto;
using GA2.Domain.Entities;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    public class ReporteSolicitudes :
        BaseAsyncEndpoint.WithRequest<RequestReporteSolicitud>
        .WithResponse<Response<DataReportSolicitud>>
    {


        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        /// <summary>
        /// Constructor de la clase del endpoint para registrar intervinientes
        /// </summary>        
        public ReporteSolicitudes(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        ///  Registro de intervinientes
        /// </summary>        
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>        
        [HttpPost("api/solicitud/DataReporteSolictud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Data reporte de solicitudes",
           Description = "Data reporte de solicitudes",
           OperationId = "Solicitud.DataReporteSolictud",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<DataReportSolicitud>>> HandleAsync(RequestReporteSolicitud request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.DataReporteSolictud(request);
        }
    }
}
