using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    public class ReportePlanillaReintegro : BaseAsyncEndpoint
        .WithRequest<GenerarPanillaPagoReintegro>
        .WithResponse<Response<FileResult>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// AgregarAfiliadoReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public ReportePlanillaReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// AfiliadoReintegroParamDto
        /// </summary>
        /// <param name="radicado"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reintegros/ReportePlanilla")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Reporte planilla pago Afiliado Reintegro",
           Description = "Reporte planilla pago Afiliado Reintegro",
           OperationId = "Reintegros.ReportePlanilla",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(GenerarPanillaPagoReintegro radicado, CancellationToken cancellationToken = default)
        {
            var userId = Guid.NewGuid().ToString();

            try
            {
                userId = User.Claims.Where(c => c.Type.Equals(USR_ID)).FirstOrDefault().Value;
            }
            catch
            {
                userId = Guid.NewGuid().ToString();

            }
            return await _reintegrosBusinessLogic.ReportePlanillaPagoReintegro(radicado,userId);
        }
    }
}
