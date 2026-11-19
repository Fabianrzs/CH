using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para actualizar parametrizacion legal tasa
    /// <author>Camilo Andres Yaya Poveda</author>
    /// <date>20/04/2021</date>
    /// </summary>
    [Authorize]
    public class ActualizarLegalTasa : BaseAsyncEndpoint.WithRequest<ParametrizacionLegalTasaDto>.WithResponse<Response<ParametrizacionLegalTasaDto>>
    {
        private readonly IParametrizacionBusinessLogic _parametrizationBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Update Legal Tasa
        /// </summary>
        /// <param name="parametrizationBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>18/05/2021</date>
        public ActualizarLegalTasa(IParametrizacionBusinessLogic parametrizationBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _parametrizationBusinessLogic = parametrizationBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/parametrizacion/ActualizarLegalTasa")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ParametrizacionLegalTasaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene parametrizacion legal tasa",
            Description = "Obtiene parametrizacion legal tasa",
            OperationId = "parametrizacion.updateLegalTasa",
            Tags = new[] { "ParametrizationEndPoint" })]
        public async override Task<ActionResult<Response<ParametrizacionLegalTasaDto>>> HandleAsync(ParametrizacionLegalTasaDto request, CancellationToken cancellationToken = default)
        {
            var tasas = await _parametrizationBusinessLogic.ObtenerLegalTasa();
            var tasaLegal = tasas.Data.Where(x => x.Id == request.Id).FirstOrDefault();     
            if (tasaLegal != null)
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
                        Modulo = "GESTION PARAMETRIZACION",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 4,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Actualizar  parametrizacion legal tasa",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var dataResponse = await _parametrizationBusinessLogic.ActualizarLegalTasa(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id,tasaLegal, request, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await _parametrizationBusinessLogic.ActualizarLegalTasa(request);
        }
    }
}