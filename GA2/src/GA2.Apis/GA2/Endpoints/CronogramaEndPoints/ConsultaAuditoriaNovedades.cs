using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using GA2.Application.Dto.AuditoriaNovedades;
using Autofac.Core;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    [Authorize]
    public class ConsultaAuditoriaNovedades :
        BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<IEnumerable<AuditoriaNovedadesGetDto>>>
    {
        private readonly IAuditoriaNovedadesBussinessLogic _auditoriaNovedadesBussinessLogic;


        public ConsultaAuditoriaNovedades(IAuditoriaNovedadesBussinessLogic auditoriaNovedadesBussinessLogic)
        {
            _auditoriaNovedadesBussinessLogic = auditoriaNovedadesBussinessLogic;
        }

        /// <summary>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/AuditoriaNovedades/ConsultarAuditoriaNovedades")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<AuditoriaNovedadesGetDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar Auditoria Novedades",
           Description = "Consultar Auditoria Novedades",
           OperationId = "AuditoriaNovedades.ConsultarAuditoriaNovedades",
           Tags = new[] { "AuditoriaNovedadesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<AuditoriaNovedadesGetDto>>>> HandleAsync(int idModulo,CancellationToken cancellationToken = default)
        {
            return await _auditoriaNovedadesBussinessLogic.ConsultaAuditoriaNovedades(idModulo);
        }
    }
}
