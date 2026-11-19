using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Endpoints.CreditoEndpoints.ValidacionIdentidad
{
    /// <summary>
    /// EndPoint para validar identidad del afiliado desde el portal
    /// </summary>
    public class ValidacionIdentidadAfiliadoPortal : BaseAsyncEndpoint
        .WithRequest<DatosIdentificacionDto>
        .WithResponse<Response<ValidacionIdentidadAfiliado>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ValidacionIdentidadAfiliadoPortal(
            ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Valida la identidad del afiliado desde el portal
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ValidacionIdentidadAfiliadoPortal")]
        [ProducesResponseType(typeof(Response<ValidacionIdentidadAfiliado>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Validación de identidad del afiliado desde el portal",
            Description = "Realiza validaciones de embargos, estado de cuenta y cuotas aportadas",
            OperationId = "credito.ValidacionIdentidadAfiliadoPortal",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ValidacionIdentidadAfiliado>>> HandleAsync(
            DatosIdentificacionDto request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ValidacionAfiliadoIdentidad(request);
        }
    }
}
