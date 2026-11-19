using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Prospeccion;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using GA2.Application.Dto.Reintegros;
using System;
using GA2.Domain.Entities.Postulacion;

namespace GA2.Apis.GA2.Endpoints.Reintegros.GestorDocumental
{
    public class InsertarComunicaciones: BaseAsyncEndpoint.WithRequest<insertarComunicacionDto>.WithResponse<Response<string>>
    {
        private readonly IReintegrosBusinessLogic _iReintegrosBusinessLogic;

        public InsertarComunicaciones(IReintegrosBusinessLogic iReintegrosBusinessLogic)
        {
            _iReintegrosBusinessLogic = iReintegrosBusinessLogic;
        }

        [HttpPost("api/Reintegros/InsertarComunicaciones")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "insertar informacion flujo 03",
           Description = "insertar informacion flujo 03",
           OperationId = "Reintegros.InsertarComunicaciones",
           Tags = new[] { "InsertarComunicaciones" })]

        public override async Task<ActionResult<Response<string>>> HandleAsync(insertarComunicacionDto reintegro, CancellationToken cancellationToken = default)
        {
            return await _iReintegrosBusinessLogic.InsertarComunicaciones(reintegro);
        }
    }
}
