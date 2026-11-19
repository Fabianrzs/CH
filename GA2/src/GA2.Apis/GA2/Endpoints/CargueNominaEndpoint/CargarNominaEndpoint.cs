using Ardalis.ApiEndpoints;
using DocumentFormat.OpenXml.Presentation;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Endpoints.FileEndpoints
{
    /// <summary>
    /// Endpoint para carga de archivos de nómina
    /// </summary>
    [Authorize]
    public class CargarNominaEndpoint : BaseAsyncEndpoint.WithRequest<CargaNominaDTO>.WithResponse<Response<IEnumerable<ArchivoDto>>>
    {
        private readonly IMovimientoBusinessLogic _movimeintoBusinessLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="movimientoBusinessLogic"></param>
        public CargarNominaEndpoint(IMovimientoBusinessLogic movimientoBusinessLogic)
        {
            _movimeintoBusinessLogic = movimientoBusinessLogic;
        }

        /// <summary>
        /// Endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cargarchivonomina")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<ArchivoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Carga Archivo Nomina",
            Description = "Carga Archivo Nomina",
            OperationId = "fileupload.CargaArchivoNomina",
            Tags = new[] { "CargueNominaEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ArchivoDto>>>> HandleAsync([FromForm] CargaNominaDTO request, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals(USR_ID)).FirstOrDefault().Value;
            if (string.IsNullOrWhiteSpace(userId)) throw new InvalidOperationException(nameof(userId));


            if (request.File == null || request.File.Length == 0)
                return BadRequest("No se ha enviado ningún archivo.");



            return await _movimeintoBusinessLogic.CargarArchivoNomina(request, Guid.Parse(userId));
        }
    }
}
