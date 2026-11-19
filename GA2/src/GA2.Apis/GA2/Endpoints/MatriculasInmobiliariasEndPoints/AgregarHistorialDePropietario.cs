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

    public class AgregarHistorialDePropietario : BaseAsyncEndpoint.WithRequest<PropietariosMatriculasInmobiliariasDto>.WithResponse<Response<PropietariosMatriculasInmobiliariasDto>>
    {

        private readonly IMatriculasInmobiliariasLogic _matriculasInmobiliariasLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        private const string USR_ID = "USR_ID";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matriculasInmobiliariasLogic"></param>
        public AgregarHistorialDePropietario(IMatriculasInmobiliariasLogic matriculasInmobiliariasLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
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
        [HttpPost("api/AgregarHistorialDePropietario")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CrearMatriculaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crea Historial de Propietario",
           Description = "Crea Historial de Propietario",
           OperationId = "matriculas.AgregarHistorialDePropietario",
           Tags = new[] { "MatriculasEndpoint" })]
        public override async Task<ActionResult<Response<PropietariosMatriculasInmobiliariasDto>>> HandleAsync(PropietariosMatriculasInmobiliariasDto request, CancellationToken cancellationToken = default)
        {
            //var userId = Guid.NewGuid().ToString();

            //try
            //{
            //    userId = User.Claims.Where(c => c.Type.Equals(USR_ID)).FirstOrDefault().Value;
            //}
            //catch
            //{
            //    userId = Guid.NewGuid().ToString();

            //}


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
                    Modulo = "MATRICULA INMOVILIARIA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion de Historial de Propietari",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await this._matriculasInmobiliariasLogic.AgregarHistorialDePropietario(request, Guid.Parse(userId));
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
           // return await this._matriculasInmobiliariasLogic.AgregarHistorialDePropietario(request,user);
        }
    }
}
