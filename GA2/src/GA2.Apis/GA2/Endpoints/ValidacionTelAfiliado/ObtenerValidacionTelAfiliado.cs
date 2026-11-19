using Ardalis.ApiEndpoints;
using GA2.Application.Dto.ValidacionTelAfiliado;
using GA2.Domain.Core.ValidacionTelAfiliado;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace GA2.Apis.GA2.Endpoints.ValidacionTelAfiliado
{
    public class ObtenerValidacionTelAfiliado : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<ValidacionTelAfiliadoDto>>
    {
        private readonly IValidacionTelAfiliadoBusinessLogic _validacionTelAfiliadoBusinessLogic;

        public ObtenerValidacionTelAfiliado(IValidacionTelAfiliadoBusinessLogic validacionTelAfiliadoBusinessLogic)
        {
            _validacionTelAfiliadoBusinessLogic = validacionTelAfiliadoBusinessLogic;
        }

        [HttpPost("api/VerificacionTelefonoAfl/ObtenerVerificacionTelefonoAfl")]
        [ProducesResponseType(typeof(Response<ValidacionTelAfiliadoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Verificacion Telefono Afiliado",
           Description = "Actualizar Verificacion Telefono Afiliado",
           OperationId = "credito.ActualizarVerificacionTelefonoAfl",
           Tags = new[] { "AfiliadoEndpoint" })]
        public override async Task<ActionResult<Response<ValidacionTelAfiliadoDto>>> HandleAsync(Guid Soc_Id, CancellationToken cancellationToken = default)
        {
            return await this._validacionTelAfiliadoBusinessLogic.ObtenerValTelAfiliado(Soc_Id);
        }
    }
}
