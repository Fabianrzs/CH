using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Clase del endpoint consultar la informacion del conyuge del afiliado.
    /// </summary>
    public class ObtenerInformacionConyuge :
         BaseAsyncEndpoint.WithRequest<InformacionConyugeDto>
        .WithResponse<Response<InformacionConyuge>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint consultar la informacion del conyuge del afiliado.
        /// </summary>
        /// <author>Alejandro Lara</author>
        /// <param name="solicitudBusinessLogic"></param>
        
        public ObtenerInformacionConyuge(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// Endpoint para consultar la informacion del conyuge 
        /// </summary>
        /// <author>Alejandro Lara</author>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/InformacionConyuge")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InformacionConyuge>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar la informacion del conyuge",
           Description = "consultar la informacion del conyuge",
           OperationId = "Solicitud.InformacionConyuge",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<InformacionConyuge>>> HandleAsync(InformacionConyugeDto request, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.InformacionConyuge(request); 
        }
    }
}
