using Ardalis.ApiEndpoints;
using GA2.Application.Dto.ValidacionTelAfiliado;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.CapXc;
using GA2.Domain.Core.PAXC;

namespace GA2.Apis.GA2.Endpoints.CapXcEndPoints
{
    public class ObtenerPAXC : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<ObtenerPAXCDto>>
    {
        private readonly IPAXCBusinessLogic _IPAXCBusinessLogic;

        public ObtenerPAXC(IPAXCBusinessLogic IPAXCBusinessLogic)
        {
            _IPAXCBusinessLogic = IPAXCBusinessLogic;
        }

        [HttpPost("api/PAXC/ObtenerPAXC")]
        [ProducesResponseType(typeof(Response<ObtenerPAXCDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Peritos y Abogados por Ciudad",
           Description = "Obtener Peritos y Abogados por Ciudad",
           OperationId = "PAXC.ObtenerPAXC",
           Tags = new[] { "PAXCEndpoint" })]
        public override async Task<ActionResult<Response<ObtenerPAXCDto>>> HandleAsync(int request, CancellationToken cancellationToken = default)
        {
            return await this._IPAXCBusinessLogic.ObtenerPAXC(request);
        }
    }
}
