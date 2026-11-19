using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints
{
    /// <summary>
    /// ObtenerConceptoPorClientePorCuentaPorConcepto
    /// </summary>
    [Authorize]
    public class ObtenerConceptoPorClientePorCuentaPorConcepto : BaseAsyncEndpoint.WithRequest<ConsultaConceptos>.WithResponse<Response<CuentaPorConceptoDto>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        /// <summary>
        /// ObtenerConceptoPorClientePorCuentaPorConcepto
        /// </summary>
        /// <param name="cuentasConceptosBusinessLogic"></param>
        public ObtenerConceptoPorClientePorCuentaPorConcepto(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
      Summary = "Obtiene un concepto con el id del cliente, id de la cuenta y id  del concepto",
         Description = "Obtiene los datos basicos de conceptos de cuentas",
         OperationId = "CuentasConceptos.ObtenerConceptoPorClienteByCuentaByConceptoBy",
         Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerConceptoPorClientePorCuentaPorConcepto")]
        public override async Task<ActionResult<Response<CuentaPorConceptoDto>>> HandleAsync([FromQuery] ConsultaConceptos request, CancellationToken cancellationToken = default)
        {
            return await _cuentasConceptosBusinessLogic.ObtenerConceptoPorClientePorCuentaPorConcepto(request.clienteId, request.cuentaId, request.conceptoId);
        }
    }
}
