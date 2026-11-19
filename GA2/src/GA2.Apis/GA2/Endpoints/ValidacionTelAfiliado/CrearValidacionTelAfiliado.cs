using Ardalis.ApiEndpoints;
using GA2.Application.Dto.ValidacionTelAfiliado;
using GA2.Domain.Core.ValidacionTelAfiliado;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.ValidacionTelAfiliado
{
    public class CrearValidacionTelAfiliado : BaseAsyncEndpoint.WithRequest<ValidacionTelAfiliadoDto>.WithResponse<Response<ValidacionTelAfiliadoDto>>
    {
        private readonly IValidacionTelAfiliadoBusinessLogic _validacionTelAfiliadoBusinessLogic;

        public CrearValidacionTelAfiliado(IValidacionTelAfiliadoBusinessLogic validacionTelAfiliadoBusinessLogic)
        {
            _validacionTelAfiliadoBusinessLogic = validacionTelAfiliadoBusinessLogic;
        }

        [HttpPost("api/VerificacionTelefonoAfl/CrearVerificacionTelefonoAfl")]
        [ProducesResponseType(typeof(Response<ValidacionTelAfiliadoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Verificacion Telefono Afiliado",
           Description = "Crear Verificacion Telefono Afiliado",
           OperationId = "credito.CrearVerificacionTelefonoAfl",
           Tags = new[] { "AfiliadoEndpoint" })]
        public override async Task<ActionResult<Response<ValidacionTelAfiliadoDto>>> HandleAsync(ValidacionTelAfiliadoDto request, CancellationToken cancellationToken = default)
        {
            return await this._validacionTelAfiliadoBusinessLogic.CrearValTelAfiliado(request);
        }
    }
}