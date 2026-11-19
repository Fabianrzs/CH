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

    public class CrearMatriculaInmobiliaria : BaseAsyncEndpoint.WithRequest<CrearMatriculaDto>.WithResponse<Response<CrearMatriculRespondeDto>>
    {

        private readonly IMatriculasInmobiliariasLogic _matriculasInmobiliariasLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matriculasInmobiliariasLogic"></param>
        public CrearMatriculaInmobiliaria(IMatriculasInmobiliariasLogic matriculasInmobiliariasLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _matriculasInmobiliariasLogic = matriculasInmobiliariasLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        ///  Crea un nuevo usuario
        /// </summary>
        /// <param name="request">Peticion usuario</param>
        /// <param name="cancellationToken">cancelacion si se desea</param>
        /// <returns>Usuario Creado</returns>
        [HttpPost("api/CrearMatriculaInmobiliaria")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CrearMatriculaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crea matricula Inmobiliaria",
           Description = "Crea matricula Inmobiliaria",
           OperationId = "matriculas.CrearMatriculaInmobiliaria",
           Tags = new[] { "MatriculasEndpoint" })]
        public override async Task<ActionResult<Response<CrearMatriculRespondeDto>>> HandleAsync(CrearMatriculaDto request, CancellationToken cancellationToken = default)
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
                    Modulo = "GESTION MATRICULA INMOVILIARIA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de matricula Inmobiliaria",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._matriculasInmobiliariasLogic.CrearMatriculaInmobiliaria(request);
            
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
           // return await this._matriculasInmobiliariasLogic.CrearMatriculaInmobiliaria(request);
        }
    }
}
