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
    public class ObtenerDatosOficioRespuesta : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<IEnumerable<TransferenciaCesantiasDto>>>
    {
        private readonly IAfiliacionesBusinessLogic _afiliacionesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afiliacionesBusinessLogic"></param>
        public ObtenerDatosOficioRespuesta(IAfiliacionesBusinessLogic afiliacionesBusinessLogic)
        {
            _afiliacionesBusinessLogic = afiliacionesBusinessLogic;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="idAfiliacion"></param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/ObtenerDatosOficioRespuesta")]
        [SwaggerOperation(
           Summary = "Obtener Datos Oficio Respuesta",
           Description = "Obtener Datos Oficio Respuesta",
           OperationId = "afiliaciones.ObtenerDatosOficioRespuesta",
           Tags = new[] { "AfiliacionesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TransferenciaCesantiasDto>>>> HandleAsync([FromQuery] int idAfiliacion, CancellationToken cancellationToken = default)
        {
            return await _afiliacionesBusinessLogic.ObtenerDatosOficioRespuesta(idAfiliacion);
        }
    }
}
