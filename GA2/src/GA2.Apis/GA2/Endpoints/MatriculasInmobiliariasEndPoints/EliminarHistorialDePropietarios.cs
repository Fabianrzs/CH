using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.MatriculasInmobiliariasEndPoints
{
    [Authorize]

    public class EliminarHistorialDePropietarios : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<PropietariosMatriculasInmobiliariasDto>>
    {

        private readonly IMatriculasInmobiliariasLogic _matriculasInmobiliariasLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matriculasInmobiliariasLogic"></param>
        public EliminarHistorialDePropietarios(IMatriculasInmobiliariasLogic matriculasInmobiliariasLogic)
        {
            _matriculasInmobiliariasLogic = matriculasInmobiliariasLogic;
        }


        /// <summary>
        ///  Crea un nuevo usuario
        /// </summary>
        /// <param name="request">Peticion usuario</param>
        /// <param name="cancellationToken">cancelacion si se desea</param>
        /// <returns>Usuario Creado</returns>
        [HttpDelete("api/EliminarHistorialDePropietarios")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PropietariosMatriculasInmobiliariasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Elimina historial de propietarios",
           Description = "Crea matricula Inmobiliaria",
           OperationId = "matriculas.EliminarHistorialDePropietarios",
           Tags = new[] { "MatriculasEndpoint" })]
        public override async Task<ActionResult<Response<PropietariosMatriculasInmobiliariasDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {

            return await this._matriculasInmobiliariasLogic.EliminarHistorialDePropietarios(request);
        }
    }
}
