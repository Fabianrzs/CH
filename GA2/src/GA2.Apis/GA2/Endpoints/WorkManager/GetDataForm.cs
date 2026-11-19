using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Core.Servicios.WorkManager;
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
    public class GetDataForm : BaseAsyncEndpoint
        .WithRequest<QueryRequestDto>
        .WithResponse<Response<Form_GetDataResponse>>
    {
        private readonly IWorkManagerService _WMServices;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="wMServices"></param>
        public GetDataForm(IWorkManagerService wMServices)
        {
            _WMServices = wMServices;
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/getdataforms")]
        [ProducesResponseType(typeof(Response<Form_GetDataResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "GetData forms",
            Description = "Service GetData Forms",
            OperationId = "credito.GetDataForms",
            Tags = new[] { "WorkManager" })]
        public async override Task<ActionResult<Response<Form_GetDataResponse>>> HandleAsync(QueryRequestDto request, CancellationToken cancellationToken = default)
        {
            return await this._WMServices.FormGetDataAsyncCH(request);
        }

    }
}
