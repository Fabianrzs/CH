using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    /// <summary>
    /// RechazarCargueNomina
    /// </summary>
    [Authorize]
    public class RechazarCargueNomina: BaseAsyncEndpoint.WithRequest<IEnumerable<DocumentoDto>>.WithResponse<Response<IEnumerable<DocumentoDto>>>
    {
        /// <summary>
        /// DI movimiento logica de negocio
        /// </summary>
        private readonly IMovimientoBusinessLogic _movimeintoBusinessLogic;

        /// <summary>
        /// Constructor Rechazar movimiento de carga 
        /// </summary>
        /// <param name="movimeintoBusinessLogic">DI movimiento logica de negocio</param>
        public RechazarCargueNomina(IMovimientoBusinessLogic movimeintoBusinessLogic)
        {
            _movimeintoBusinessLogic = movimeintoBusinessLogic;
        }

        /// <summary>
        /// Procesar archivos de carga endpoint
        /// </summary>
        /// <param name="request">Lista de archivos cargados</param>
        /// <param name="cancellationToken">Cancelacion de peticion</param>
        /// <returns>Lista de archivos procesados</returns>
        [HttpPost("api/rechazarcarguenomina")]
        [ProducesResponseType(typeof(Response<IEnumerable<DocumentoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Rechazar archivos cargados",
            Description = "Rechazar Archivos Nomina",
            OperationId = "rechazar.rechazarcarguenomina",
            Tags = new[] { "CargueNominaEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<DocumentoDto>>>> HandleAsync(IEnumerable<DocumentoDto> request, CancellationToken cancellationToken = default)
        {
            return await _movimeintoBusinessLogic.RechazarCargueNomina(request);
        }
    }
}


