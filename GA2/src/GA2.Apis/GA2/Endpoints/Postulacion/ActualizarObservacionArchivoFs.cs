using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints
{
    public class ActualizarObservacionArchivoFs : BaseAsyncEndpoint.WithRequest<ActualizarObservacionDto>
        .WithResponse<Response<string>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ActualizarObservacionArchivoFs(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/updateObservacionArchivo")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar la observacion de archivo de fondo solidaridad",
            Description = "Actualizar  la observacion de archivoa de fondo solidaridad",
            OperationId = "postulante.updateObservacionArchivo",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(ActualizarObservacionDto request, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ActualizarObservacionArchivos(request);
        }
    }
}
