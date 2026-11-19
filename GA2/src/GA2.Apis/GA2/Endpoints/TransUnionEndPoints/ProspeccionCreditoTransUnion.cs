using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.TransUnionEndPoints
{
    public class ProspeccionCreditoTransUnion : BaseAsyncEndpoint.WithRequest<ProspeccionTransUnionRequestDto>.WithResponse<Response<ResponseTu>>
    {
        private readonly IGestorTransUnionBusinessLogic _gestorTransUnionBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="gestorTransUnionBusinessLogic"></param>
        public ProspeccionCreditoTransUnion(IGestorTransUnionBusinessLogic gestorTransUnionBusinessLogic)
        {
            _gestorTransUnionBusinessLogic = gestorTransUnionBusinessLogic;
        }

        /// <summary>
        /// verificacionTercero
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/ProspeccionCredito")]
        [ProducesResponseType(typeof(Response<ClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "ProspeccionCredito",
            Description = "Verificacion Prospeccion Credito",
            OperationId = "Credito.ProspeccionCredito",
            Tags = new[] { "Credito" })]
        public async override Task<ActionResult<Response<ResponseTu>>> HandleAsync(ProspeccionTransUnionRequestDto requestTU, CancellationToken cancellationToken = default)
        {
            return await this._gestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion(requestTU);
        }

    }
}
