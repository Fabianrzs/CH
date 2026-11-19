using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Postulacion;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ConsultarObservacionArchivoEndPoint : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<ObservacionArchivoDto>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ConsultarObservacionArchivoEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpGet("api/postulante/consultarObservacionById")]
        [ProducesResponseType(typeof(Response<ObservacionArchivoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar la observacion de arhivos de fondo solidaridad",
            Description = "Consultar la observacion de arhivos de fondo solidaridad",
            OperationId = "postulante.consultarObservacionById",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<ObservacionArchivoDto>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ConsultarObservacionArchivos(request);
        }
    }
}
