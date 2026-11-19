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

namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints
{
    [Authorize]

    public class ObtenerEntidadEducativaDetalle : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<EntidadEducativaDto>>
    {
        private readonly IEntidadesBusinessLogic _catalogsBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Obtener Entidad Educativa
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public ObtenerEntidadEducativaDetalle(IEntidadesBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }
        /// <summary></summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Entidades/ObtenerEntidadEducativaDetalle")]
        [ProducesResponseType(typeof(Response<IEnumerable<EntidadEducativaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene el detalle de una entidad educativa",
           Description = "Obtiene el detalle de una entidad educativa",
           OperationId = "Entidades.ObtenerEntidadEducativaDetalle",
           Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<EntidadEducativaDto>>> HandleAsync(Guid idEntidad, CancellationToken cancellationToken = default)
        {
            return await this._catalogsBusinessLogic.ObtenerEntidadEducativaDetalle(idEntidad);
        }
    }
}
