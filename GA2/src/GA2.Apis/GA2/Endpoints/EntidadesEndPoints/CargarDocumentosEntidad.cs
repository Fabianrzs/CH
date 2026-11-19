using System.Linq;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace GA2.Apis.GA2.Endpoints.EntidadesEndPoints
{
    /// <summary>
    /// Endpoint carga documentos entidades educativas
    /// </summary>
    [Authorize]
    public class CargarDocumentosEntidad : BaseAsyncEndpoint.WithRequest<IFormCollection>.WithResponse<Response<bool>>
    {
        private readonly IEntidadesBusinessLogic _entidadesBusinessLogic;

        public CargarDocumentosEntidad(IEntidadesBusinessLogic entidadesBusinessLogic)
        {
            _entidadesBusinessLogic = entidadesBusinessLogic;
        }

        [HttpPost("api/Entidades/CargarDocumentosEntidad/{idEntidad}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation("Carga Archivo al blob storage de las entidades",
            Description = "Carga Archivo al blob storage de las entidades",
            OperationId = "Entidades.CargarDocumentosEntidad",
            Tags = new[] { "EntidadesEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(IFormCollection request, CancellationToken cancellationToken = default)
        {
            List<CargarDocumentosEntidadDto> listaArchivos = new List<CargarDocumentosEntidadDto>();
            Guid idEntidad = Guid.Parse((Request.Path.Value.Split("/")[Request.Path.Value.Split("/").Length - 1]).Split("$")[0]);

            foreach (var i in Request.Form.Files)
            {
                var archivoParametrizadoId = i.FileName.Split("--")[0];
                listaArchivos.Add(new CargarDocumentosEntidadDto
                {
                    archivoParametrizadoId = new Guid(archivoParametrizadoId),
                    archivoCargado = i,
                    numeroFolio = i.FileName.Split("--")[2]
                });
            }

            return await _entidadesBusinessLogic.CargarDocumentosEntidad(listaArchivos, idEntidad);
        }
    }
}
