using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.Comite
{
    /// <summary>
    /// Genera un flujo de firmas para un 
    /// </summary>
    [Authorize]

    public class CrearFlujoFirmasComite:BaseAsyncEndpoint.WithRequest<RequestFlujoFirmasDto>.WithResponse<object>
    {
        private readonly IGestorDocumentalBusinessLogic _gestorDocumentalBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="gestorDocumentalBusinessLogic"></param>
        public CrearFlujoFirmasComite(IGestorDocumentalBusinessLogic gestorDocumentalBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic)
        {
            _gestorDocumentalBusinessLogic = gestorDocumentalBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
        }

        /// <summary>
        /// insertForm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/crearFlujoFirmasComite")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear Flujo Firmas Comite",
            Description = "Crear Flujo Firmas Comite",
            OperationId = "credito.CrearFlujoFirmasComite",
            Tags = new[] { "WorkManager" })]
        public async override Task<ActionResult<object>> HandleAsync(RequestFlujoFirmasDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;

            if (name == null)
            {
                return await _gestorDocumentalBusinessLogic.CrearFlujoFirmasComite(request);
            }

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
                    Descripcion = "Guardar Informacion Flujo Firmas Comite",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var dataResponse = await _gestorDocumentalBusinessLogic.CrearFlujoFirmasComite(request);
                var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, request, null, userId);
                return dataResponse;
            }
            catch (System.Exception e)
            {
                throw;
            }
            return await _gestorDocumentalBusinessLogic.CrearFlujoFirmasComite(request);
        }
    }
}
