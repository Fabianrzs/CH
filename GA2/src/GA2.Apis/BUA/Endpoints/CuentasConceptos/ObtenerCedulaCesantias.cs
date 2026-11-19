using Ardalis.ApiEndpoints;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Transversals.Commons;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class ObtenerCedulaCesantias : BaseAsyncEndpoint
                                                          .WithoutRequest
                                                          .WithResponse<Response<IEnumerable<DatosCedulasCesantiasDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerCedulaCesantias(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos de cedula de las cuentas Cesantias",
           Description = "Obtiene los datos de cedula de las cuentas Cesantias",
           OperationId = "CuentasConceptos.ObtenerCedulaCuentaCesantias",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerCedulaCuentaCesantias")]
        public async override Task<ActionResult<Response<IEnumerable<DatosCedulasCesantiasDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ObtenerCedulaCuentaCesantidas();
        }
    }
}
