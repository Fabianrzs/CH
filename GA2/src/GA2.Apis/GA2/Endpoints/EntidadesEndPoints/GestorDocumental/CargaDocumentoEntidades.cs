using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints.GestorDocumental
{
    public class CargaDocumentoEntidades : BaseAsyncEndpoint.WithRequest<IFormFile>.WithResponse<Response<string>>
    {
        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entidadesBusinessLogic"></param>
        public CargaDocumentoEntidades(IEntidadesBusinessLogic entidadesBusinessLogic)
        {
            _entidadesBusinessLogic = entidadesBusinessLogic;
        }

        /// <summary>
        /// Carga Archivo al blob storage para Entidades credito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Entidades/CargaDocumentoEntidades")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation("Carga Archivo al blob storage para entidades, Crear Documento entidades",
            Description = "Carga Archivo al blob storage para entidades",
            OperationId = "Entidades.CargaDocumentoEntidades",
            Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(IFormFile request, CancellationToken cancellationToken = default)
        {
            return await _entidadesBusinessLogic.CargaDocumentoEntidades(request);
        }
    }
}
