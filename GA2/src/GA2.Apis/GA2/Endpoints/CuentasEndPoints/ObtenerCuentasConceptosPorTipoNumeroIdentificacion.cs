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

namespace GA2.Apis.GA2.Endpoints.CuentasEndPoints
{
    [Authorize]

    public class ObtenerCuentasConceptosPorTipoNumeroIdentificacion : BaseAsyncEndpoint.WithRequest<DocumentoDTO>.WithResponse<Response<InfoClienteDto>>
    {


        private readonly ICuentasClientesBusinessLogic _cuentasClientesBusinessLogic;
        public ObtenerCuentasConceptosPorTipoNumeroIdentificacion(ICuentasClientesBusinessLogic cuentasClientesBusinessLogic)
        {
            _cuentasClientesBusinessLogic = cuentasClientesBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Trae las cuentas y los conceptos del usuario por tipo numero de identificacion",
           Description = "Trae las cuentas y los conceptos del usuario por tipo numero de identificacion",
           OperationId = "CuentasClientes.ObtenerCuentasConceptosPorTipoNumeroIdentificacion",
           Tags = new[] { "CuentasClientes" })]
        [HttpGet("api/ObtenerCuentasConceptosPorTipoNumeroIdentificacion")]
        public override async Task<ActionResult<Response<InfoClienteDto>>> HandleAsync([FromQuery]  DocumentoDTO request, CancellationToken cancellationToken = default)
        {
            return await _cuentasClientesBusinessLogic.ObtenerCuentasConceptosPorTipoNumeroIdentificacion(request.tipoDocumentoId, request.numeroDocumento);

             
        }
    }
}
