using Ardalis.ApiEndpoints;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class ObtenerCedulaCuentaAVAC : BaseAsyncEndpoint
                                                          .WithoutRequest
                                                          .WithResponse<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerCedulaCuentaAVAC(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos de cedula de las cuentas AVAC",
           Description = "Obtiene los datos de cedula de las cuentas AVAC",
           OperationId = "CuentasConceptos.ObtenerCedulaCuentaAVAC",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerCedulaCuentaAVAC")]
        public async override Task<ActionResult<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ObtenerCedulaCuentaAVAC();
        }
    }
}
