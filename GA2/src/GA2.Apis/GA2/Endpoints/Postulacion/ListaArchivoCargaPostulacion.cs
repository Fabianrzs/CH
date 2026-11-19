using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    public class ListaArchivoCargaPostulacion :
        BaseAsyncEndpoint.WithRequest<DocumentoModalidadPostulacionDto>
        .WithResponse<Response<IEnumerable<TipoListaCargarArchivoPostulacionDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ListaArchivoCargaPostulacion(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/listaArchivoPostulante")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener la lista de los archivo a cargar",
            Description = "Obtener  toda la lista de los archivo a cargar",
            OperationId = "postulante.listaArchivoPostulante",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoListaCargarArchivoPostulacionDto>>>> HandleAsync(DocumentoModalidadPostulacionDto documento , CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ObtenerListaArchivoPorModalidad(documento);
        }
    }
}
