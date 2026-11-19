using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Catalogs.ComprobantesCartera;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints.ComprobantesCartera.TipoComprobante
{
    public class ObtenerTipoComprobante : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<TipoComprobanteDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        public ObtenerTipoComprobante(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerTipoComprobanteCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoComprobanteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Tipo Comprobante Cartera",
            Description = "Obtener Tipo Comprobante Cartera",
            OperationId = "catalogos.ObtenerTipoComprobanteCartera",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoComprobanteDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _catalogosBusinessLogic.ObtenerTipoComprobanteCartera();
        }
    }
}
