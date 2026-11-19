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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    [Authorize]

    public class ObtenerOficinasDeRegistro : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<OficinaDeRegistroDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        public ObtenerOficinasDeRegistro(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }

        /// <summary>
        /// ObtenerOficinasDeRegistro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerOficinasDeRegistro")]
        [ProducesResponseType(typeof(Response<OficinaDeRegistroDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener oficinas de registro",
            Description = "Obtener oficinas de registro",
            OperationId = "catalogos.ObtenerOficinasDeRegistro",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<OficinaDeRegistroDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _catalogosBusinessLogic.ObtenerOficinasDeRegistro();
        }
    }
}
