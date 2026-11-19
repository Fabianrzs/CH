using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Main.ProviderRequest;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using GA2.Domain.Entities;
using System.Net;
using System.Text.RegularExpressions;

namespace GA2.Apis.BUA.Endpoints
{
    /// <summary>
    /// Endpoint que actualiza los datos de usuario
    /// <author>Nicolas Florez Sarrazola</author>
    /// </summary>
    [Authorize]
    public class ActualizarCliente : BaseAsyncEndpoint.WithRequest<ClienteDto>.WithResponse<Response<ClienteDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;
        private readonly IAuditoriaProvider _auditoriaProvider;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public ActualizarCliente(IClientesBusinessLogic clientsBusinessLogic, IAuditoriaProvider auditoriaProvider)
        {
            _clientsBusinessLogic = clientsBusinessLogic;
            _auditoriaProvider = auditoriaProvider;
        }

        /// <summary>
        /// Update client info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        //[ValidateAntiForgeryToken]
        [HttpPatch("api/clientes")]
        [ProducesResponseType(typeof(Response<ClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza los datos de un cliente",
           Description = "Actualiza un cliente",
           OperationId = "client.actualizarcliente",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<ClienteDto>>> HandleAsync(ClienteDto request, CancellationToken cancellationToken = default)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "USR_NOMBRE")?.Value;
            var surnNme = User.Claims.Where(c => c.Type.Equals("USR_APELLIDO")).FirstOrDefault().Value;
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            var user = name + " " + surnNme;
            var ip = _clientsBusinessLogic.Ip();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var dataNavegador = _clientsBusinessLogic.ParseUserAgent(userAgent);


            try
            {
                var requestAuditoria = new AuditoriaDto()
                {
                    Modulo = "CLIENTES",
                    FechaRegistro = DateTime.Now,
                    Hostname = ip,
                    IdTipoAccion = 6,
                    SistemaOperativo = dataNavegador.OperatingSystem,
                    Descripcion = "Guardar informacion  de un cliente",
                    Navegador = dataNavegador.Browser,
                    UsurioId = Guid.Parse(userId),
                    NombreUsuario = user,
                    TipoAccion = "GUARDAR"

                };
                var response = await _clientsBusinessLogic.ActualizarCliente(request);
                await _auditoriaProvider.GuardarAuditoria(requestAuditoria);
                return response;
            }
            catch (Exception ex) { }
            return await _clientsBusinessLogic.ActualizarCliente(request);

        }
    }
}
