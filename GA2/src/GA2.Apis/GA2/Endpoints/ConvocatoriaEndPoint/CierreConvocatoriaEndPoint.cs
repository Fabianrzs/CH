using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class CierreConvocatoriaEndPoint : BaseAsyncEndpoint.WithRequest<CierreConvocatoriaDto>
        .WithResponse<Response<IEnumerable<PostulanteConvocatoriaDto>>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public CierreConvocatoriaEndPoint(IConvocatoriaBussigneLogic estadosBusinessLogic)
        {
            _convocatoriaBussigneLogic = estadosBusinessLogic;
        }
        /// <summary>
        /// Crear la solicitud de convocatoria
        /// </summary>
        /// <param name="convocatoriaDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/convocatoria/CrearCierreConvocatoria")]
        [ProducesResponseType(typeof(Response<IEnumerable<PostulanteConvocatoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear cierre convocatoria",
           Description = "Crear cierre  convocatoria",
           OperationId = "convocatoria.CrearCierreConvocatoria",
           Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<PostulanteConvocatoriaDto>>>> HandleAsync(CierreConvocatoriaDto convocatoriaDTO, CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.CrearCierreConvocatoria(convocatoriaDTO);
        }
    }
}
