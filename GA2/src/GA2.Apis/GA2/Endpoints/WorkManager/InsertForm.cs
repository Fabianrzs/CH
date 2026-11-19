using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class InsertForm:BaseAsyncEndpoint.WithRequest<PeticionCreditoBasicaDto>.WithResponse<Response<Form_InsertResponse>>
    {
        private readonly IGestorDocumentalBusinessLogic _WMBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="wMBusinessLogic"></param>
        public InsertForm(IGestorDocumentalBusinessLogic wMBusinessLogic)
        {
            _WMBusinessLogic = wMBusinessLogic;
        }

        /// <summary>
        /// insertForm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/insertForm")]
        [ProducesResponseType(typeof(Response<Form_InsertResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insert forms",
            Description = "Service Insert Forms",
            OperationId = "credito.InsertForm",
            Tags = new[] { "WorkManager" })]
        public async override Task<ActionResult<Response<Form_InsertResponse>>> HandleAsync(PeticionCreditoBasicaDto request, CancellationToken cancellationToken = default)
        {
            return await this._WMBusinessLogic.CrearRegistro(request);
        }
    }
}
