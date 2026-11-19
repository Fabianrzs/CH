using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Escrituras;
using GA2.Domain.Core.FichaAnalista;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.FichaAnalista;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.FichaAnalistaEndPoints
{
    [Authorize]
    public class ObtenerFichaAnalista : BaseAsyncEndpoint.WithRequest<ObtenerFichaAnalistaDto>.WithResponse<Response<IEnumerable<FichaAnalistaDto>>>    {
        private readonly IFichaAnalistaBusinessLogic _IfichaAnalistaBusinessLogic;
        public ObtenerFichaAnalista(IFichaAnalistaBusinessLogic IfichaAnalistaBusinessLogic)
        {
            _IfichaAnalistaBusinessLogic = IfichaAnalistaBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/FichaAnalista/ObtenerFichaAnalista")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<FichaAnalistaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Ficha Analista",
            Description = "Obtener Ficha Analista",
            OperationId = "fichaAnalista.ObtenerFichaAnalista",
            Tags = new[] { "FichaAnalistaEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<FichaAnalistaDto>>>> HandleAsync(ObtenerFichaAnalistaDto FichaAnalista, CancellationToken cancellationToken = default)
        {
            return await this._IfichaAnalistaBusinessLogic.ObtenerFichaAnalista(FichaAnalista);
        }

    }
}
