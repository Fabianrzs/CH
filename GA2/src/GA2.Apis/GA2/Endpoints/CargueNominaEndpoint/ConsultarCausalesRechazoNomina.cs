using Ardalis.ApiEndpoints;
using Azure;
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
    public class ConsultarCausalesRechazoNomina: BaseAsyncEndpoint.WithoutRequest.WithResponse<Transversals.Commons.Response<IEnumerable<CausalRechazoNomina>>>
    {
        private readonly IDocumentoBusinessLogic _documentoBusinessLogic;

        /// <summary>
        /// Consultar las causales de rechazo de nomina
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        /// <author>Alejandro Lara Miranda</author>
        public ConsultarCausalesRechazoNomina(IDocumentoBusinessLogic documentoBusinessLogic)
        {
            _documentoBusinessLogic = documentoBusinessLogic;
        }

        /// <summary>
        /// Consultar las causales de rechazo de nomina
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/ConsultarCausalesRechazoNomina")]
        [ProducesResponseType(typeof(Transversals.Commons.Response<IEnumerable<CausalRechazoNomina>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar las causales de rechazo de nomina",
            Description = "Consultar las causales de rechazo de nomina",
            OperationId = "fileupload.ConsultarCausalesRechazoNomina",
            Tags = new[] { "CargueNominaEndpoint" })]
        public override async Task<ActionResult<Transversals.Commons.Response<IEnumerable<CausalRechazoNomina>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _documentoBusinessLogic.CausalRechazoNomina();
        }
    }
}
