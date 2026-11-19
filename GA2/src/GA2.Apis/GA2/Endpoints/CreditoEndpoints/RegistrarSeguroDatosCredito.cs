using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class RegistrarSeguroDatosCredito : BaseAsyncEndpoint.WithRequest<SegurosDatosSolicitudCreditoDto>.WithResponse<Response<SegurosDatosSolicitudCredito>>
    {

        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Constructor para consultar seguros
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public RegistrarSeguroDatosCredito(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }


        [HttpPost("api/prospeccion/RegistrarSeguroDatosCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SegurosDatosSolicitudCredito>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Registrar seguros de solicitud de credito",
         Description = "Registrar seguros de solicitud de credito",
         OperationId = "prospeccion.RegistrarSeguroDatosCredito",
         Tags = new[] { "ProspeccionEndpoint" })]
        public override async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<Response<SegurosDatosSolicitudCredito>>> HandleAsync(SegurosDatosSolicitudCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.RegistrarSeguroSolicitudCredito(request); 
        }
    }
}
