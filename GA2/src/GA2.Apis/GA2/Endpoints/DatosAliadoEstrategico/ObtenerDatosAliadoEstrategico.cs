using Ardalis.ApiEndpoints;
using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Domain.Core.DatosAliadoEstrategico;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace GA2.Apis.GA2.Endpoints.DatosAliadoEstrategico
{
    public class ObtenerDatosAliadoEstrategico : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<DatosAliadoEstrategicoDto>>
    {
        private readonly IDatosAliadoEstrategicoBusinessLogic _datosAliadoEstrategicoBusinessLogic;
        public ObtenerDatosAliadoEstrategico(IDatosAliadoEstrategicoBusinessLogic datosAliadoEstrategicoBusinessLogic)
        {
            _datosAliadoEstrategicoBusinessLogic = datosAliadoEstrategicoBusinessLogic;
        }

        [HttpPost("api/ObtenerDatosAliadoEstrategico")]
        [ProducesResponseType(typeof(Response<DatosAliadoEstrategicoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Aliado Estrategico",
           Description = "Obtener Aliado Estrategico",
           OperationId = "credito.ObtenerDatosAliadoEstrategico",
           Tags = new[] { "AliadoEstrategicoEndpoint" })]
        public override async Task<ActionResult<Response<DatosAliadoEstrategicoDto>>> HandleAsync(Guid Soc_id, CancellationToken cancellationToken = default)
        {
            return await this._datosAliadoEstrategicoBusinessLogic.ObtenerDatosAliadoEstrategico(Soc_id);
        }
    }
}
