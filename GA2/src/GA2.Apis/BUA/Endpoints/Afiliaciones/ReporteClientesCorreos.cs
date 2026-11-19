using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
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
    /// Endpoint que obtiene los clientes y sus respectivos correos electronicos
    /// </summary>
    [Authorize]
    public class ReporteClientesCorreos : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<ReporteClienteCorreosErradosDto>>>
    {
        private readonly IAfiliacionesBusinessLogic _afiliacionesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afiliacionesBusinessLogic"></param>
        public ReporteClientesCorreos(IAfiliacionesBusinessLogic afiliacionesBusinessLogic)
        {
            _afiliacionesBusinessLogic = afiliacionesBusinessLogic;
        }

        /// <summary>
        /// Metodo Que obtiene los clientes con correos electronicos para la verificacion de errados
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/ClienteCorreosErrados")]
        [SwaggerOperation(
           Summary = "Se obtienen los correos errados de los clientes para un reporte",
           Description = "Cliente Correos Errados",
           OperationId = "afiliaciones.ClienteCorreosErrados",
           Tags = new[] { "AfiliacionesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteClienteCorreosErradosDto>>>> HandleAsync( CancellationToken cancellationToken = default)
        {
            return await _afiliacionesBusinessLogic.ObtenerClientesCorreoErrados();
        }
    }
}
