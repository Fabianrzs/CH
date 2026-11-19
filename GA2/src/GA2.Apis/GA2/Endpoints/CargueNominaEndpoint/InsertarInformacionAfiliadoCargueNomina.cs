using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    public class InsertarInformacionAfiliadoCargueNomina
    : BaseAsyncEndpoint.WithRequest<DatosInformacionCliente>.WithResponse<Response<string>>
    {
        private readonly IDocumentoBusinessLogic _documentoBusinessLogic;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="documentoBusiness"></param>
        public InsertarInformacionAfiliadoCargueNomina(IDocumentoBusinessLogic documentoBusiness)
        {
            _documentoBusinessLogic = documentoBusiness;
        }
        /// <summary>
        /// Endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/InsertarInformacionAfiliadoCargueNomina")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Validar Inconsistencias de Nomina frente Bua",
            Description = "Validar Inconsistencias de Nomina",
            OperationId = "fileupload.InsertarInformacionAfiliadoCargueNomina",
            Tags = new[] { "CargueNominaEndpoint" })]
        public override async Task<ActionResult<Response<string>>> HandleAsync(DatosInformacionCliente request, CancellationToken cancellationToken = default)
        {
            return await _documentoBusinessLogic.InsertarInformacionAfiliadoCargueNomina(request); 
        }
    }
}
