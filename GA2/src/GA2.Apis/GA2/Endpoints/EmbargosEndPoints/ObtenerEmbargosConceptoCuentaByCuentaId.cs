using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    [Authorize]

    public class ObtenerEmbargosConceptoCuentaByCuentaId : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<IEnumerable<EmbargoCuentaConceptoDto>>>
    {

        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;

        public ObtenerEmbargosConceptoCuentaByCuentaId(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }


        [HttpGet("api/ObtenerEmbargosConceptoCuentaByCuentaId")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<EmbargoCuentaConceptoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Listar todos los beneficiarios de pago de embargos",
            Description = "Listar todos los beneficiarios de pago de embargos",
            OperationId = "Embargos.ObtenerBeneficiariosEmbargo",
            Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<IEnumerable<EmbargoCuentaConceptoDto>>>> HandleAsync(int idCuenta, CancellationToken cancellationToken = default)
        {
            return await this._embargosBusinessLogic.ObtenerEmbargosConceptoCuentaByCuentaId(idCuenta);
        }
    }
}
