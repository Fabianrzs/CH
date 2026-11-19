using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Entidades;
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

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Crear una Entidad Educativa
    /// </summary>
    /// <author>Edwin Lopez</author>
    /// <date>15/03/2022</date>

    [Authorize]
    public class EliminarFideicomiso :
        BaseAsyncEndpoint.WithRequest<IdentificadoresEntidadEducativa>
        .WithResponse<Response<IEnumerable<FidecomisoDto>>>

    {

        private readonly IEntidadesBusinessLogic _catalogsBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint CrearFideicomisoEntidadEducativa
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public EliminarFideicomiso(IEntidadesBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// Crear Entidad Educativa
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Entidades/EliminarFideicomiso")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<FidecomisoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Elimina Fidecomiso Entidad Educativa",
            Description = "Elimina Fidecomiso Entidad Educativa",
            OperationId = "Entidades.EliminarFideicomiso",
            Tags = new[] { "EntidadesEndpoint" })]

        public async override Task<ActionResult<Response<IEnumerable<FidecomisoDto>>>> HandleAsync(IdentificadoresEntidadEducativa request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var user = name + " " + surnNme;
            var ip = _auditoriaBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);


            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "ENTIDAD",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Eliminar informacion Fidecomiso Entidad Educativa",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
                var dataResponse = await _catalogsBusinessLogic.EliminarFideicomiso(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse.Data, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {


                return await _catalogsBusinessLogic.EliminarFideicomiso(request);

            }

        }
    }
}

