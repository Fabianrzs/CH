using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Prospeccion
{
    //[Authorize]
    public class ObtenerDocumentosRequeridosSolicCredito : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<DocumentoCargarSolicitudDto>>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Ctor e inyeccion dependencias
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ObtenerDocumentosRequeridosSolicCredito(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        /// <summary>
        /// Obtener Documentos Requeridos Solic Credito
        /// </summary>
        /// <param name = "cancellationToken" > cancelacion si se desea</param>
        [HttpGet("api/prospeccion/ObtenerDocumentosRequeridosSolicCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DocumentoCargarSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Documentos Requeridos Solic Credito",
           Description = "Obtener Documentos Requeridos Solic Credito",
           OperationId = "prospeccion.ObtenerDocumentosRequeridosSolicCredito",
           Tags = new[] { "ProspeccionEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<DocumentoCargarSolicitudDto>>>> HandleAsync(Guid idSolicitud, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ObtenerDocumentosRequeridosSolicCredito(idSolicitud);
        }
    }
}
