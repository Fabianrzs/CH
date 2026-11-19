using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.AuditoriaNovedades;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    [Authorize]
    public class ValidaNovedadesFondoSolidaridad :
        BaseAsyncEndpoint.WithRequest<RequestValidaNovedadesFondoSolidaridadDto>.WithResponse<Response<ResponseValidaNovedadesFondoSolidaridadDto>>
    {
        private readonly IAuditoriaNovedadesBussinessLogic _auditoriaNovedadesBussinessLogic;

        public ValidaNovedadesFondoSolidaridad(IAuditoriaNovedadesBussinessLogic auditoriaNovedadesBussinessLogic)
        {
            _auditoriaNovedadesBussinessLogic = auditoriaNovedadesBussinessLogic;
        }

        /// <summary>
        /// </summary>
        /// <param name = "request"></param >
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cronograma/ValidaNovedadesFondoSolidaridad")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<AuditoriaNovedadesDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "ValidaNovedadesFondoSolidaridad",
           Description = "ValidaNovedadesFondoSolidaridad",
           OperationId = "AuditoriaNovedades.ValidaNovedadesFondoSolidaridad",
           Tags = new[] { "AuditoriaNovedadesEndpoint" })]
        public async override Task<ActionResult<Response<ResponseValidaNovedadesFondoSolidaridadDto>>> HandleAsync(RequestValidaNovedadesFondoSolidaridadDto request, CancellationToken cancellationToken = default)
        {
            return await this._auditoriaNovedadesBussinessLogic.ValidaNovedadesFondoSolidaridad(request);
        }
    }
}
