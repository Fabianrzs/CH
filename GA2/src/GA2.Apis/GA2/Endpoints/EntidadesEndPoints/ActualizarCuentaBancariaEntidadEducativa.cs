using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Actualizar Cuenta Bancaria Entidad Educativa
    /// </summary>
    /// <author>Edwin Lopez</author>
    /// <date>11/05/2021</date>

    [Authorize]
    public class ActualizarCuentaBancariaEntidadEducativa :
         BaseAsyncEndpoint.WithRequest<CuentaBancariaEntidadEducativaDto>
        .WithResponse<Response<CuentaBancariaEntidadEducativaDto>>
    {

        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;
        private readonly IAuditoriaBusinessLogic _auditoriaBusinessLogic;
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Actualizar Entidad Educativa
        /// </summary>
        /// <param name="entidadesBusinessLogic"></param>
        /// <author>Edwin Lopez</author>
        /// <date>11/05/2021</date>
        public ActualizarCuentaBancariaEntidadEducativa(IEntidadesBusinessLogic entidadesBusinessLogic, IAuditoriaBusinessLogic auditoriaBusinessLogic, ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _entidadesBusinessLogic = entidadesBusinessLogic;
            _auditoriaBusinessLogic = auditoriaBusinessLogic;
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }

        /// <summary>
        /// Actualizar Cuenta Bancaria Entidad Educativa
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("api/Entidades/ActualizarCuentaBancariaEntidadEducativa")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CuentaBancariaEntidadEducativaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar Cuenta Bancaria Entidad Educativa",
            Description = "Actualizar Cuenta Bancaria Entidad Educativa",
            OperationId = "Entidades.ActualizarCuentaBancariaEntidadEducativa",
            Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<CuentaBancariaEntidadEducativaDto>>> HandleAsync(CuentaBancariaEntidadEducativaDto request, CancellationToken cancellationToken = default)
        {
            var obtenerCuentas = await _catalogosBusinessLogic.ObtenerCuentaBancariaPorEntidad(request.IdEntidad);
            var cuentaBancaria = obtenerCuentas.Data.Where(x => x.IdEntidadBancaria == request.IdEntidadBancaria && x.NumeroCuenta == request.NumeroCuenta).FirstOrDefault();
            if (cuentaBancaria is not null)
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
                        Descripcion = "Actualiza  Cuenta Bancaria Entidad Educativa",
                        Navegador = dataNavegador.Browser,
                        UsurioId = Guid.Parse(userId),
                        NombreUsuario = user,
                        TipoAccion = "MODIFICACIONES"

                    };
                    var response = await _entidadesBusinessLogic.ActualizarCuentaBancariaEntidadEducativa(request);
                    var transation = await _auditoriaBusinessLogic.InsertarLogAuditoria(requestAuditoria);
                    await _auditoriaBusinessLogic.LogAuditAsync(transation.Data.Id, cuentaBancaria, request, userId);
                    return response;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            return await _entidadesBusinessLogic.ActualizarCuentaBancariaEntidadEducativa(request);
        }
    }
}

