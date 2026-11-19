using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.AuditoriaEndPoints
{
    
    public class InsertarLogAuditoria : BaseAsyncEndpoint.WithRequest<AuditoriaDto>.WithResponse<Response<AuditoriaDto>>
    {
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        public InsertarLogAuditoria(IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// Insertar Log de Auditoria
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/InsertarLogAuditoria")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<AuditoriaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Inserta el log de auditoria",
            Description = "Onserta el log de auditoria",
            OperationId = "Auditoria.InsertarLogAuditoria",
            Tags = new[] { "AuditoriaEndPoints" })]
        public override async Task<ActionResult<Response<AuditoriaDto>>> HandleAsync(AuditoriaDto request, CancellationToken cancellationToken = default)
        {
            return await _auditoriaBusinessLogic.InsertarLogAuditoria(request);
        }
    }
}
