using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    public class ConsultaNotificacionesCargueNomina : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<ConsultarNotificacionNomina>>>
    {
        private readonly IMovimientoBusinessLogic _movimientoBusinessLogic;

        /// <param name="movimientoBusinessLogic"></param>
        /// <author>Alejandro Lara Miranda</author>
        public ConsultaNotificacionesCargueNomina(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimientoBusinessLogic = movimientoBusinessLogic;
        }

        /// <summary>
        /// Consultar notificacion cargue nomina
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/ConsultaNotificacionesCargueNomina")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConsultarNotificacionNomina>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar notificacion cargue nomina",
            Description = "Consultar notificacion cargue nomina",
            OperationId = "fileupload.ConsultaNotificacionesCargueNomina",
            Tags = new[] { "CargueNominaEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ConsultarNotificacionNomina>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _movimientoBusinessLogic.ConsultarNotificacionNominaSolicitud(request); 
        }
    }
}
