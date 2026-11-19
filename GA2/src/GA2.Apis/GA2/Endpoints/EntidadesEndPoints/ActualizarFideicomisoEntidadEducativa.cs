using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
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
    public class ActualizarFideicomisoEntidadEducativa :
        BaseAsyncEndpoint.WithRequest<FidecomisoDto>
        .WithResponse<Response<IEnumerable<FidecomisoDto>>>

    {

        private readonly IEntidadesBusinessLogic _catalogsBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint CrearFideicomisoEntidadEducativa
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        public ActualizarFideicomisoEntidadEducativa(IEntidadesBusinessLogic catalogsBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
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
        [HttpPost("api/Entidades/ActualizarFideicomisoEntidadEducativa")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<FidecomisoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Actualiza Fidecomiso Entidad Educativa",
            Description = "Actualiza Fidecomiso Entidad Educativa",
            OperationId = "Entidades.CrearFideicomisoEntidadEducativa",
            Tags = new[] { "EntidadesEndpoint" })]

        public async override Task<ActionResult<Response<IEnumerable<FidecomisoDto>>>> HandleAsync(FidecomisoDto request, CancellationToken cancellationToken = default)
        {
            var fideicomiso = await _catalogsBusinessLogic.ObtenerCuentaBancariaPorEntidad(request.entidad_id.ToString());
            var data = fideicomiso.Data.Where(x => x.ENE_FIDEICOMISO_ID == request.ENE_FIDEICOMISO_ID).FirstOrDefault();
            if (data != null)
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
                        Modulo = "GESTIÓN DE ENTIDADES EDUCATIVA",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 4,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Actualizar Fidecomiso Entidad Educativa",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var response = await _catalogsBusinessLogic.ActualizarFideicomisoEntidadEducativa(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, data, request, userId);
                    return response;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await _catalogsBusinessLogic.ActualizarFideicomisoEntidadEducativa(request);

        }


    }
}

