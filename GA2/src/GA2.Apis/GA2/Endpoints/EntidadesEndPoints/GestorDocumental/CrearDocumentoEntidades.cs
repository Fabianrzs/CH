using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Entidades;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints.GestorDocumental
{
    public class CrearDocumentoEntidades: BaseAsyncEndpoint.WithRequest<RequestDocumentoEntidadesDto>.WithResponse<Response<ResponseDocumentoSolicitudDto>>
    {
        
        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entidadesBusinessLogic"></param>
        public CrearDocumentoEntidades(IEntidadesBusinessLogic entidadesBusinessLogic)
        {
            _entidadesBusinessLogic = entidadesBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Entidades/CrearDocumentoEntidades")]
        [ProducesResponseType(typeof(Response<ResponseDocumentoSolicitudDto>), StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [SwaggerOperation(
            Summary = "Crear Documento Entidades",
            Description = "Obtener Entidades Credito Informacion Tecnica",
            OperationId = "Entidades.CrearDocumentoEntidades",
            Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<ResponseDocumentoSolicitudDto>>> HandleAsync([FromBody] RequestDocumentoEntidadesDto request, CancellationToken cancellationToken = default)
        {
            //var file = Request.Form.Files[0];
            return await this._entidadesBusinessLogic.CrearDocumentoEntidades(request);
        }
    }
}
