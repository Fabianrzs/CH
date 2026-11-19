using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class RechazarDocumentoFsEndPoint : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<SolicituPostulancionResponseDto>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public RechazarDocumentoFsEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/RechazarCargeDocumento")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Rechazar el cargue de documento de fondo de solidaridad",
            Description = "Rechazar el cargue de documento de fondo de solidaridad",
            OperationId = "postulante.RechazarCargeDocumento",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<SolicituPostulancionResponseDto>>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.RechazarCargueDocumento(id);
        }
    }
}
