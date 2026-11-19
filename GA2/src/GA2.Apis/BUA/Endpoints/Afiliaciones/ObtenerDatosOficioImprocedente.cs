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
    public class ObtenerDatosOficioImprocedente : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<IEnumerable<OficioImprocedenteDto>>>
    {
        private readonly IAfiliacionesBusinessLogic _afiliacionesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afiliacionesBusinessLogic"></param>
        public ObtenerDatosOficioImprocedente(IAfiliacionesBusinessLogic afiliacionesBusinessLogic)
        {
            _afiliacionesBusinessLogic = afiliacionesBusinessLogic;
        }

        /// <summary>
        /// ObtenerDatosOficioImprocedente
        /// </summary>
        /// <param name="idAfiliacion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/ObtenerDatosOficioImprocedente")]
        [SwaggerOperation(
           Summary = "Reporte Informacion Cliente",
           Description = "Reporte Informacion Cliente",
           OperationId = "afiliaciones.ObtenerDatosOficioImprocedente",
           Tags = new[] { "AfiliacionesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<OficioImprocedenteDto>>>> HandleAsync([FromQuery] int idAfiliacion, CancellationToken cancellationToken = default)
        {
            return await _afiliacionesBusinessLogic.ObtenerDatosOficioImprocedente(idAfiliacion);
        }
    }
}
