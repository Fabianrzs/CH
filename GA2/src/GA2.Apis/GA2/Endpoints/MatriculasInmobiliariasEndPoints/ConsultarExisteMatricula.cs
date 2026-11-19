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

namespace GA2.Apis.GA2.Endpoints.MatriculasInmobiliariasEndPoints
{
    /// <summary>
    /// Endpoint para consultar si existe una matrícula inmobiliaria
    /// </summary>
    /// <author>Hanson Restrepo</author>
    [Authorize]
    public class ConsultarExisteMatricula :
         BaseAsyncEndpoint.WithRequest<string>
        .WithResponse<Response<MatriculaInmobiliariaRespuestaDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para consultar si existe una matrícula inmobiliaria
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>05/05/2022</date>
        public ConsultarExisteMatricula(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para consultar si existe una matrícula inmobiliaria
        /// </summary>
        /// <param name="numeroMatricula"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/solicitud/ConsultarExisteMatricula")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<MatriculaInmobiliariaRespuestaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar existe Matricula",
           Description = "Consultar existe Matricula",
           OperationId = "Solicitud.ConsultarExisteMatricula",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<MatriculaInmobiliariaRespuestaDto>>> HandleAsync(string numeroMatricula, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ConsultarExisteMatricula(numeroMatricula);
        }
    }
}