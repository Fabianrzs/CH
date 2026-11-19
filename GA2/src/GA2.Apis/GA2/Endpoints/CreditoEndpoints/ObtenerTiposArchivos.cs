using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
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
    /// Endpoint para Obtener Tipo Documento
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>11/05/2021</date>
    [Authorize]
    public class ObtenerTiposArchivos : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<TiposArchivosDto>>>
    {
        private readonly ICreditoBusinessLogic _tipodocumentoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Obtener Tipo Documento
        /// </summary>
        /// <param name="tipodocumentoBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public ObtenerTiposArchivos(ICreditoBusinessLogic tipodocumentoBusinessLogic)
        {
            _tipodocumentoBusinessLogic = tipodocumentoBusinessLogic;
        }


        /// <summary>
        /// Obtiene all ciudades
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerTiposArchivos")]
        [ProducesResponseType(typeof(Response<IEnumerable<TiposArchivosDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene tipos de archivo",
            Description = "Obtiene tipos de archivo",
            OperationId = "credito.obtenertiposarchivos",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TiposArchivosDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {

            return await this._tipodocumentoBusinessLogic.ObtenerTiposArchivos();
        }

    }
}
