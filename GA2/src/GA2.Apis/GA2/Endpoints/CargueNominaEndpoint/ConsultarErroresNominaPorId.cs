using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Endpoints.CargueNominaEndpoint
{
    [Authorize]
    public class ConsultarErroresNominaPorId : BaseAsyncEndpoint.WithRequest<ConsultarArchivosIdDto>.WithResponse<Response<List<ErrorCargueArchivoDto>>>
    {
        private readonly IMovimientoBusinessLogic _movimientoBusinessLogic;

        public ConsultarErroresNominaPorId(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimientoBusinessLogic = movimientoBusinessLogic;
        }

        [HttpPost("api/ConsultarErroresNomina")]
        [ProducesResponseType(typeof(Response<List<ErrorCargueArchivoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar Errores Nomina ID",
            Description = "Consultar Errores Nomina ID",
            OperationId = "fileupload.ConsultarErroresNomina",
            Tags = new[] { "CargueNominaEndpoint" })]
        //public async override Task<ActionResult<Response<List<ErrorCargueArchivo>>>> HandleAsync([FromBody] int request, CancellationToken cancellationToken = default)
        public async override Task<ActionResult<Response<List<ErrorCargueArchivoDto>>>> HandleAsync(ConsultarArchivosIdDto request, CancellationToken cancellationToken = default)

        {
            return await this._movimientoBusinessLogic.ConsultarErroresNominaPorId(request);
        }
    }
}