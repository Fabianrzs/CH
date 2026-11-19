using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class SaveGenerarPagoReintegro : BaseAsyncEndpoint
        .WithRequest<DataGenerarPagoDto>
        .WithResponse<Response<bool>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;
        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public SaveGenerarPagoReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/GenerarPagoReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Guardar la generacion de pago para Reintegro",
           Description = "Guardar la generacion de pago para",
           OperationId = "Reintegros.GenerarPagoReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(DataGenerarPagoDto request, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.GenerarPagoData(request);
        }
    }
}
