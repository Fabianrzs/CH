using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    /// <summary>
    /// Endpoint para inconsistencias de archivos de nómina
    /// </summary>
    [Authorize]

    public class ValidarNominaBua : BaseAsyncEndpoint.WithRequest<ConsultarArchivosIdDto>.WithResponse<Response<List<ErrorCargueArchivoDto>>>
    {
        private readonly IMovimientoBusinessLogic _movimeintoBusinessLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        public ValidarNominaBua(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimeintoBusinessLogic = movimientoBusinessLogic;
        }
        /// <summary>
        /// Endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/ValidarNominaBua")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<ErrorCargueArchivoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Validar Inconsistencias de Nomina frente Bua",
            Description = "Validar Inconsistencias de Nomina",
            OperationId = "fileupload.ValidarNominaBua",
            Tags = new[] { "CargueNominaEndpoint" })]

        public async override Task<ActionResult<Response<List<ErrorCargueArchivoDto>>>> HandleAsync(ConsultarArchivosIdDto request, CancellationToken cancellationToken = default)

        //public override async Task<ActionResult<Response<SolicitudNominaDto>>> HandleAsync(SolicitudNominaDto request, CancellationToken cancellationToken = default)
        {
            return await _movimeintoBusinessLogic.ValidarNominaBua(request);
        }
    }
}
