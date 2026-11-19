using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.CronogramaEndPoints
{
    public class EliminarNovedades :
         BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<NovedadesCronogramaDto>>
    {

        private readonly ICronogramaNovedadesBusinessLogic _cronogramaBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Alerta
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>11/05/2021</date>
        public EliminarNovedades(ICronogramaNovedadesBusinessLogic cronogramaNovedadesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _cronogramaBusinessLogic = cronogramaNovedadesBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;   
        }


        /// <summary>
        /// </summary>
        /// <param name="editarCronogramaDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpDelete("api/cronograma/EliminarNovedades")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CronogramaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Editar fecha de reporte de un cronograma",
           Description = "Editar fecha de reporte de un cronograma",
           OperationId = "Cronograma.editarFechaDeReporte",
           Tags = new[] { "CronogramaEndpoint" })]
        public async override Task<ActionResult<Response<NovedadesCronogramaDto>>> HandleAsync(int idNovedad, CancellationToken cancellationToken = default)
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
                    Modulo = "CRONOGRAMA",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 7,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Eliminar informacion de la noveda",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "ELIMINAR"

                };
              //  var dataDelete = _cronogramaBusinessLogic.ObtenerNovedadAfiliadoPorId(idNovedad);
                var dataResponse = await this._cronogramaBusinessLogic.EliminarNovedad(idNovedad);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, dataResponse.Data, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return await this._cronogramaBusinessLogic.EliminarNovedad(idNovedad);
        }
    }
}
