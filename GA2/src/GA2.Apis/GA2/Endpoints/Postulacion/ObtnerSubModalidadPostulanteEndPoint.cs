
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ObtnerSubModalidadPostulanteEndPoint : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<IEnumerable<TipoSubModalidadPostulacionDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ObtnerSubModalidadPostulanteEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpGet("api/postulante/ConsultarTipoSubModaliadPostulante")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoSubModalidadPostulacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene todos los tipo de modalidad",
            Description = "Obtiene todos los tipo de modaliad para la postulacion ",
            OperationId = "postulante.ConsultarTipoSubModaliadPostulante",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoSubModalidadPostulacionDto>>>> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ConsultarSubModalidad(id);
        }
    }
}
