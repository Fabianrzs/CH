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
    public class ObtenerDatosCedulaAVC : BaseAsyncEndpoint
                                                          .WithoutRequest
                                                          .WithResponse<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerDatosCedulaAVC(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos de cedula de las cuentas AVC",
           Description = "Obtiene los datos de cedula de las cuentas AVC",
           OperationId = "CuentasConceptos.ObtenerCedulaCuentaAVC",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerCedulaCuentaAVC")]
        public async override Task<ActionResult<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ObtenerCedulaCuentaAVC();
        }
    }
}
