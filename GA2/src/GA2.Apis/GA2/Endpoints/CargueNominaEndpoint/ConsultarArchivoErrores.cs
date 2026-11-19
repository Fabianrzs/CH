using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
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
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class consultarArchivosConError : BaseAsyncEndpoint.WithRequest<ConsultarArchivosConErrorDto>.WithResponse<Response<List<ArchivoConErrorDto>>>
    {
        private readonly IMovimientoBusinessLogic _movimientoBusinessLogic;

        /// <summary>
        /// Movimiento business logic
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        /// <date>19/05/2021</date>
        /// <author>Oscar Julian Rojas</author>
        public consultarArchivosConError(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimientoBusinessLogic = movimientoBusinessLogic;
        }

        /// <summary>
        /// Obtener archivos cargados
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/consultararchivosconerror")]
        [ProducesResponseType(typeof(Response<ArchivoConErrorDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener archivos cargados que tengan error",
            Description = "Obtener Archivos Nomina con Error",
            OperationId = "fileupload.consultararchivosconerror",
            Tags = new[] { "CargueNominaEndpoint" })]
        public async override Task<ActionResult<Response<List<ArchivoConErrorDto>>>> HandleAsync(ConsultarArchivosConErrorDto request, CancellationToken cancellationToken = default)
        {
            return await this._movimientoBusinessLogic.ConsultarArchivoConError(request);
        }
    }
}