using Ardalis.ApiEndpoints;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    public class AprobarDocumentoCargueNominaSolicitud : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<DocumentoDto>>
    {
        /// <summary>
        /// DI movimiento logica de negocio
        /// </summary>
        private readonly IDocumentoBusinessLogic _documentoBusinessLogic;

        /// <summary>
        /// Constructor Aprobar Documento
        /// </summary>
        /// <param name="movimeintoBusinessLogic">DI movimiento logica de negocio</param>
        public AprobarDocumentoCargueNominaSolicitud(IDocumentoBusinessLogic documentoBusinessLogic)
        {
            _documentoBusinessLogic = documentoBusinessLogic;
        }

        /// <summary>
        /// Aprobar archivos de carga endpoint
        /// </summary>
        /// <param name="request">documento id</param>
        /// <param name="cancellationToken">Cancelacion de peticion</param>
        /// <returns>Archivo aprobado</returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/AprobarDocumentoCargueNominaSolicitud")]
        [ProducesResponseType(typeof(Response<IEnumerable<ArchivoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Procesar archivos cargados",
            Description = "Procesar Archivos Nomina",
            OperationId = "fileupload.procesararchivoscargados",
            Tags = new[] { "CargueNominaEndpoint" })]
        public override async Task<ActionResult<Response<DocumentoDto>>> HandleAsync(int request, CancellationToken cancellationToken = default)
        {
            return await _documentoBusinessLogic.AprobarCargueNominaSolicitud(request);

        }
    }
}
