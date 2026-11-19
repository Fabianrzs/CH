
using Ardalis.ApiEndpoints;
using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Core.InfoVendedor;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using System.Collections.Generic;


namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.InfVendedorEndpoint
{
    [Authorize]
    public class ObtenerInformacionVendedor : BaseAsyncEndpoint.WithRequest<consultarVendedorDto>.WithResponse<Response<IEnumerable<InformacionVendedorDto>>>
    {
        private readonly IInformacionVendedorBusinessLogic _informacionVendedorBusinessLogic;

        public ObtenerInformacionVendedor(IInformacionVendedorBusinessLogic informacionVendedorBusinessLogic)
        {
            _informacionVendedorBusinessLogic = informacionVendedorBusinessLogic;
        }


        [HttpPost("api/credito/ObtenerInformacionVendedor")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<InformacionVendedorDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Informacion Vendedor",
           Description = "Obtener Informacion Vendedor",
           OperationId = "InfVendedor.ObtenerInformacionVendedor",
           Tags = new[] { "VendedorEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<InformacionVendedorDto>>>> HandleAsync(consultarVendedorDto request, CancellationToken cancellationToken = default)
        {
            return await _informacionVendedorBusinessLogic.ObtenerInformacionVendedor(request);
        }
    }
}
