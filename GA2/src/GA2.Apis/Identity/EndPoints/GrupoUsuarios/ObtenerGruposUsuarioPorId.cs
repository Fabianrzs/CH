using Ardalis.ApiEndpoints;
using GA2.Application.Dto.GrupoUsuarios;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace GA2.Apis.Identity.EndPoints.GrupoUsuarios
{
    [Authorize]
    public class ObtenerGruposUsuarioPorId : BaseAsyncEndpoint
        .WithRequest<Guid>.WithResponse<Response<IEnumerable<ResponseGruposUsuarioPorId>>>
    {
        private readonly IGruposUsuariosLogic _gruposusuariosLogic;

        public ObtenerGruposUsuarioPorId(IGruposUsuariosLogic gruposusuariosLogic)
        {
            _gruposusuariosLogic = gruposusuariosLogic;
        }
        /// <summary>
        ///  busca los grupos a los cuales pertenece el usuario
        /// </summary>
        /// <param name="request">Peticion usuario</param>
        /// <param name="cancellationToken">cancelacion si se desea</param>
        /// <returns>Usuario Creado</returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/ObtenerGruposUsuariosPorId")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ResponseGruposUsuarioPorId>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "busca los grupos a los cuales pertenece el usuario",
           Description = "busca los grupos a los cuales pertenece el usuario",
           OperationId = "grupos.ObtenerGruposUsuariosPorId",
           Tags = new[] { "GrupoUsuariosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ResponseGruposUsuarioPorId>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._gruposusuariosLogic.ObtenerGruposUsuarioPorId(request);
        }
    }
}
