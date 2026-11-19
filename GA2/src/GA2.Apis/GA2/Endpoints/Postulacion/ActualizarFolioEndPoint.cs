using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Postulacion;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ActualizarFolioEndPoint : BaseAsyncEndpoint.WithRequest<ActualizarFolioDto>
        .WithResponse<Response<string>>


    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public ActualizarFolioEndPoint(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/ActualizarFolio")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar los folios  de fondo solidaridad",
            Description = "Actualizar  los folios de fondo solidaridad",
            OperationId = "postulante.ActualizarFolio",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(ActualizarFolioDto request, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ActualizarFolio(request.IdSolicitud,request.NumeroFolios);
        }
    }
}
