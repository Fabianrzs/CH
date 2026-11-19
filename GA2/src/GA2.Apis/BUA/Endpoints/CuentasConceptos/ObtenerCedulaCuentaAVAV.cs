using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class ObtenerCedulaCuentaAVAV :BaseAsyncEndpoint
                                                          .WithoutRequest
                                                          .WithResponse<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

    public ObtenerCedulaCuentaAVAV(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
    {
        _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
    }


    [SwaggerOperation(
    Summary = "Obtiene los datos de cedula de las cuentas AVAV",
       Description = "Obtiene los datos de cedula de las cuentas AVAV",
       OperationId = "CuentasConceptos.ObtenerCedulaCuentaAVAV",
       Tags = new[] { "CuentasConceptosEndPoints" })]
    [HttpGet("api/ObtenerCedulaCuentaAVAV")]
    public async override Task<ActionResult<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>>> HandleAsync( CancellationToken cancellationToken = default)
    {

        return await _cuentasConceptosBusinessLogic.ObtenerCedulaCuentaAVAV();
    }
}
}
