using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using GA2.Infraestructure.Repositories;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ObtenerBeneficiarioPostulacion : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<PostulanteResponseDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;

        /// <summary>
        /// Constructor del Endpoint consultar archivo por solicitud
        /// </summary>
        /// <param name="PostulacionBussineLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>07/03/2022</date>
        public ObtenerBeneficiarioPostulacion(IPostulacionBussineLogic postulacion)
        {
            _postulacionBussineLogic = postulacion;
        }

        /// <summary>
        /// Consulta Archivo al blob storage para solicitud de cesantías
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/postulante/ObtenerBeneficiarioPostulacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<PostulanteResponseDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar los beneficiario por solicitud",
           Description = "Consultar los beneficiario por solicitud",
           OperationId = "postulante.ObtenerBeneficiarioPostulacion",
           Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<PostulanteResponseDto>>>> HandleAsync(Guid IdSolicitud, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ObtenerPostulanteById(IdSolicitud);
        }
    }
}