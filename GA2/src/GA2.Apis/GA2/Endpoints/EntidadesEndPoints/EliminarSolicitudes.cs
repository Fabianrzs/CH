using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Entidades;

namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints
{
    public class EliminarSolicitudes : BaseAsyncEndpoint.WithRequest<string>
        .WithResponse<Response<String>>
    {
        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;

        public EliminarSolicitudes(IEntidadesBusinessLogic entidadesBusinessLogic)
        {
            _entidadesBusinessLogic = entidadesBusinessLogic;
        }

        [HttpGet("api/Entidades/EliminarSolicitudes")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<TramiteEntidadesDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Elimina Tramites de una cedula restaurada",
           Description = "Elimina tramites de una cedula restaurada",
           OperationId = "Entidades.ConsultarArchivosPorEntidad",
           Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(string Identificacion, CancellationToken cancellationToken = default)
        {
            return await this._entidadesBusinessLogic.EliminarSolicitudes(Identificacion);
        }
    }
}
