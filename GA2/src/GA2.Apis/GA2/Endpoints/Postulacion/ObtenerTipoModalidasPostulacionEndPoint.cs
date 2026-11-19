using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ObtenerTipoModalidasPostulacionEndPoint : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<IEnumerable<TipoModaliadPostulacionDto>>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ObtenerTipoModalidasPostulacionEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpGet("api/postulante/ConsultarTipoModaliadPostulante")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoModaliadPostulacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene todos los tipo de modalidad",
            Description = "Obtiene todos los tipo de modaliad para la postulacion ",
            OperationId = "postulante.ConsultarTipoModaliadPostulante",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoModaliadPostulacionDto>>>> HandleAsync(int id,CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ConsultarModalidad(id);
        }
    }
}
