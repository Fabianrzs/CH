using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion.Tipo;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ObtenerTipoDiscapacidaPostulacion : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<TipoICatalogoPostulacionDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;

        /// <summary>
        /// Constructor del Endpoint consultar archivo por solicitud
        /// </summary>
        /// <param name="PostulacionBussineLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>07/03/2022</date>
        public ObtenerTipoDiscapacidaPostulacion(IPostulacionBussineLogic postulacion)
        {
            _postulacionBussineLogic = postulacion;
        }

        /// <summary>
        /// </summary>
        /// <param name=""></param>
        /// <param name="+"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/ObtenerTipoDiscapacidaPostulacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<TipoICatalogoPostulacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar tipo de discapacidad de postulacion ",
           Description = "Consultar etapa de postulacion",
           OperationId = "postulante.ObtenerTipoDiscapacidaPostulacion",
           Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoICatalogoPostulacionDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ObtenerTipoDiscapacidaPostulacion();
        }
    }
}
