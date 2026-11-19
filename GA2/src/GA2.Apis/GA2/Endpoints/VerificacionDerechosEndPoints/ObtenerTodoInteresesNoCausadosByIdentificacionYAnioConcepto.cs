using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para obtener parametrizacion legal tasa
    /// <author>Camilo Andres Yaya Poveda</author>
    /// <date>20/04/2021</date>
    /// </summary>
    [Authorize]
    public class ObtenerTodoInteresesNoCausadosByIdentificacionYAnioConcepto : BaseAsyncEndpoint
                                                .WithRequest<InteresNoCausadoDto>
                                                .WithResponse<Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>>
    {

        private readonly IVerificacionDerechosBusinessLogic _verificacionDerechosBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Crear Valor IPC
        /// </summary>
        /// <param name="verificacionDerechosBusinessLogic"></param>
        /// <author>Cesar Hernandez</author>
        /// <date>18/05/2021</date>
        public ObtenerTodoInteresesNoCausadosByIdentificacionYAnioConcepto(IVerificacionDerechosBusinessLogic verificacionDerechosBusinessLogic)
        {
            _verificacionDerechosBusinessLogic = verificacionDerechosBusinessLogic;
        }

        /// <summary>
        /// Obtener parametrizacion de tasas
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/VerificacionDerechos/ObtenerTodoInteresesNoCausadosByIdentificacionYAnioConcepto")]
        [ProducesResponseType(typeof(Response<InteresesCausadosNoCausadosInfoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene los intereses causados/Nocausados por numero de identificación",
            Description = "Obtiene los intereses causados/Nocausados por numero de identificación y por año seleccionado",
            OperationId = "VerificacionDerechos.getObtenerTodoInteresesNoCausadosByIdentificacionYAnioConcepto",
            Tags = new[] { "VerificacionDerechosEndPoints" })]
        public async override Task<ActionResult<Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>>> HandleAsync([FromQuery] InteresNoCausadoDto request, CancellationToken cancellationToken = default)
        {
            return await _verificacionDerechosBusinessLogic.ObtenerTodoInteresesNoCausadosByIdentificacionYAnioYConcepto(request);

        }
       

    }
}
