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

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints.ComprobantesCartera.Beneficiarios
{
    public class ObtenerTipoBeneficiario : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<TipoBeneficiarioDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        public ObtenerTipoBeneficiario(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerBeneficiarioCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoBeneficiarioDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Beneficiario Cartera",
            Description = "Obtener Beneficiario Cartera",
            OperationId = "catalogos.ObtenerBeneficiarioCartera",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoBeneficiarioDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _catalogosBusinessLogic.ObtenerBeneficiarioCartera();
        }
    }
}
