using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class CrearEntrevistaEndPoint : BaseAsyncEndpoint.WithRequest<EntrevistaDto>
        .WithResponse<Response<string>>

    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public CrearEntrevistaEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/crearEntrevistas")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear la entrevista de fondo solidaridad",
            Description = "Crear  la entrevista de fondo solidaridad",
            OperationId = "postulante.crearEntrevistas",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(EntrevistaDto request, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.CrearEntrevista(request);
        }
    }
}
