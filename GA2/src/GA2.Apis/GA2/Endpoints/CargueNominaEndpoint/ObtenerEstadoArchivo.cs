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
    public class ObtenerEstadosArchivo : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<EstadoArchivoDto>>>
    {
        private readonly IMovimientoBusinessLogic _movimientoBusinessLogic;

        /// <summary>
        /// Movimiento business logic
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        /// <date>19/05/2021</date>
        /// <author>Oscar Julian Rojas</author>
        public ObtenerEstadosArchivo(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimientoBusinessLogic = movimientoBusinessLogic;
        }

        /// <summary>
        /// Obtener estados documentos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/ObtenerEstadosArchivo")]
        [ProducesResponseType(typeof(Response<IEnumerable<EstadoArchivoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener estados archivo",
            Description = "Obtener Estados Archivo",
            OperationId = "fileupload.ObtenerEstadosArchivo",
            Tags = new[] { "CargueNominaEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<EstadoArchivoDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._movimientoBusinessLogic.ObtenerEstadosArchivos();
        }
    }
}
