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
    [Authorize]

    public class ObtenerCuentasConceptosPorTipoNumeroIdentidad : BaseAsyncEndpoint.WithRequest<DocumentoDTO>.WithResponse<Response<InfoClienteDto>>
    {

        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerCuentasConceptosPorTipoNumeroIdentidad(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }

        [SwaggerOperation(
        Summary = "Trae las cuentas y los conceptos del usuario por tipo numero de identificacion",
           Description = "Trae las cuentas y los conceptos del usuario por tipo numero de identificacion",
           OperationId = "CuentasConceptos.ObtenerCuentasConceptosPorTipoNumeroIdentidad",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerCuentasConceptosPorTipoNumeroIdentidad")]
        public override async Task<ActionResult<Response<InfoClienteDto>>> HandleAsync([FromQuery]  DocumentoDTO request, CancellationToken cancellationToken = default)
        {
            return await _cuentasConceptosBusinessLogic.ObtenerCuentasConceptosPorTipoNumeroIdentidad(request.tipoDocumentoId, request.numeroDocumento);
        }
    }
}
