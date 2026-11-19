using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Convocatoria;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class UpdateFechaCierre : BaseAsyncEndpoint.WithRequest<UpdateFechaCierreDto>
        .WithResponse<Response<string>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public UpdateFechaCierre(IConvocatoriaBussigneLogic estadosBusinessLogic)
        {
            _convocatoriaBussigneLogic = estadosBusinessLogic;
        }
        /// <summary>
        /// Crear la solicitud de convocatoria
        /// </summary>
        /// <param name="convocatoriaDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/convocatoria/UpdateFechaCierre")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear convocatoria",
           Description = "Crear convocatoria",
           OperationId = "convocatoria.UpdateFechaCierre",
           Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(UpdateFechaCierreDto convocatoriaDTO, CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.UpdateFechaCierre(convocatoriaDTO);
        }
    }
}
