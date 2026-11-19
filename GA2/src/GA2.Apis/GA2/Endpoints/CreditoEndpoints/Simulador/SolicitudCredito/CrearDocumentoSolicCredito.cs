using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Endpoints.CreditoEndpoints.Simulador.SolicitudCredito
{
    /// <summary>
    /// Crear documentos solicitud credito
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
    [Authorize]
    public class CrearDocumentoSolicCredito : BaseAsyncEndpoint.WithRequest<RequestDocumentoSolicCreditoDto>.WithResponse<Response<ResponseDocumentoSolicCreditoDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearDocumentoSolicCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/crearDocumentoSolicCredito")]
        [ProducesResponseType(typeof(Response<ResponseDocumentoSolicCreditoDto>), StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [SwaggerOperation(
            Summary = "Crear Documento Solicitud Credito",
            Description = "Obtener Solicitud Credito Informacion Tecnica",
            OperationId = "credito.CrearDocumentoSolicCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<ResponseDocumentoSolicCreditoDto>>> HandleAsync([FromBody]RequestDocumentoSolicCreditoDto request, CancellationToken cancellationToken = default)
        {
            //var file = Request.Form.Files[0];
            return await this._creditoBusinessLogic.CrearDocumentoSolicCredito(request);
        }
    }
}
