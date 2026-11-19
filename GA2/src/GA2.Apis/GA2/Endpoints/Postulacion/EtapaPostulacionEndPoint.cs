using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.Postulacion.Tipo;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class EtapaPostulacionEndPoint : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<TipoEstadoSolicitudPostulacionDto>>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public EtapaPostulacionEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/etapasPostulacion")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoEstadoSolicitudPostulacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
    Summary = "Obtiene todos las etapas de la postulacion",
    Description = "Obtiene todos los tipo de de las etapa de la postulacion",
    OperationId = "postulante.etapasPostulacion",
    Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoEstadoSolicitudPostulacionDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.TipoEstadoSolicitudPostulacion();
        }
    }
}
