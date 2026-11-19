using Ardalis.ApiEndpoints;
using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Core.InfoVendedor;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.InfVendedorEndpoint
{
    [Authorize]
    public class EliminarInformacionVendedor : BaseAsyncEndpoint.WithRequest<RequestEliminarVendedorCredito>.WithResponse<Response<IEnumerable<InformacionVendedorDto>>>
    {
        private readonly IInformacionVendedorBusinessLogic _informacionVendedorBusinessLogic;

        public EliminarInformacionVendedor(IInformacionVendedorBusinessLogic informacionVendedorBusinessLogic)
        {
            _informacionVendedorBusinessLogic = informacionVendedorBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpDelete("api/credito/EliminarInformacionVendedor")]
        [ProducesResponseType(typeof(Response<IEnumerable<InformacionVendedorDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation("Eliminar Informacion Vendedor",
           Description = "Eliminar Informacion Vendedor",
           OperationId = "credito.EliminarInformacionVendedor",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<InformacionVendedorDto>>>> HandleAsync([FromQuery] RequestEliminarVendedorCredito request, CancellationToken cancellationToken = default)
        {
            return await _informacionVendedorBusinessLogic.EliminarInformacionVendedor(new InformacionVendedorDto { Id = request.Id });
        }
    }
}
