using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.TransUnionEndPoints
{
    public class ValidacionCodigoOtpIdVision : BaseAsyncEndpoint.WithRequest<RequestTuDto>.WithResponse<Response<ResponseTu>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        /// <summary>
        /// Ctor e inyeccion dependencias
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public ValidacionCodigoOtpIdVision(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        /// <summary>
        ///  Obtiene un producto por su id
        /// </summary>
        /// <param name = "request" > Peticion usuario</param>
        /// <param name = "cancellationToken" > cancelacion si se desea</param>
        /// <returns>Usuario Creado</returns>
        [HttpPost("api/prospeccion/ValidacionCodigoOtpIdVision")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Valida datos de otp idvision",
           Description = "Valida datos de otp idvision",
           OperationId = "prospeccion.ValidacionCodigoOtpIdVision",
           Tags = new[] { "ProspeccionEndpoint" })]

        public async override Task<ActionResult<Response<ResponseTu>>> HandleAsync(RequestTuDto request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ValidacionCodigoOtpIdVision(request);
        }
    }
}
