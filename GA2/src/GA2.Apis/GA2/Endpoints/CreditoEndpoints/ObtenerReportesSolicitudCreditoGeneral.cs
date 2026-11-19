using Ardalis.ApiEndpoints;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Core.Reportes.DiseñoReportes.TransUnion;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ObtenerReportesSolicitudCreditoGeneral :
         BaseAsyncEndpoint.WithRequest<ReportesSolicitudCreditoDto>
        .WithResponse<Response<IEnumerable<ReportesSolicitudCredito>>>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Obtener Estado Actividad Por Id
        /// </summary>
        /// <param name="estadoactividadBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ObtenerReportesSolicitudCreditoGeneral(ICreditoBusinessLogic businessLogic)
        {
            _creditoBusinessLogic = businessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ObtenerReportesSolicitudCreditoGeneral")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EstadoActividadDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener ObtenerReportesSolicitudCreditoGeneral",
           Description = "Obtener informacion de creditos",
           OperationId = "credito.ObtenerReportesSolicitudCreditoGeneral",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReportesSolicitudCredito>>>> HandleAsync(ReportesSolicitudCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ObtenerReportesSolicitudCreditoGeneral(request); 
        }
    }
}
