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

namespace GA2.Endpoints.CreditoEndpoints.Simulador.SolicitudCredito
{
    /// <summary>
    /// Enpoint para crear simulacion del cliente
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
    [Authorize]
    public class CrearSimulacionCliente : BaseAsyncEndpoint.WithRequest<CreacionSimulacionClienteDto>.WithResponse<Response<IEnumerable<SimulacionDto>>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prospeccionBusinessLogic"></param>
        public CrearSimulacionCliente(IProspeccionBusinessLogic prospeccionBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }


        /// <summary>
        /// crea simulacion del cliente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("/api/prospeccion/CrearSimulacionCliente")]
        [ProducesResponseType(typeof(Response<IEnumerable<SimulacionDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation("Crear Simulacion Cliente",
            Description = "Crear Simulacion Clienteo",
            OperationId = "prospeccion.CrearSimulacionCliente",
            Tags = new[] { "ProspeccionEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<SimulacionDto>>>> HandleAsync(CreacionSimulacionClienteDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;

            if (name == null)
            {
                
                    var userId = "83E91A3C-02A3-4D49-995E-87154620F98D";
                    var user = "AFILIADO";
                    var ip = _auditoriaBusinessLogic.Ip();
                    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                    var dataNavegador = _auditoriaBusinessLogic.ParseUserAgent(userAgent);
                try
                {
                    var requestAuditoria = new AuditoriaDto()
                    {
                        Modulo = "CREDITO",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 6,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Guardar Informacion Simulacion Cliente",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await _prospeccionBusinessLogic.CrearSimulacionCliente(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }
            else
            {
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
                        Modulo = "CREDITO",
                        FechaRegistro = DateTime.Now,
                        Hostname = ip,
                        IdTipoAccion = 6,
                        SistemaOperativo = dataNavegador.OperatingSystem,
                        Descripcion = "Guardar Informacion Simulacion Cliente",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "GUARDAR"

                    };
                    var dataResponse = await _prospeccionBusinessLogic.CrearSimulacionCliente(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                    return dataResponse;
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }

            return await _prospeccionBusinessLogic.CrearSimulacionCliente(request);
        }
    }
}
