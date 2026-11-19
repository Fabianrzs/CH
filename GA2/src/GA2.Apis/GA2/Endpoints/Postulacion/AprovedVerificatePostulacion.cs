using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Postulacion;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class AprovedVerificatePostulacion : BaseAsyncEndpoint.WithRequest<VeficateFondoDto>
        .WithResponse<Response<string>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public AprovedVerificatePostulacion(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/AprovedVerificateNovedadesFs")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Aprobar verificacion de novedades postulacion",
            Description = "Aprobar la verificacion de postulacion",
            OperationId = "postulante.AprovedVerificateNovedadesFs",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(VeficateFondoDto verificate, CancellationToken cancellationToken = default)
        {
            var userId = Guid.NewGuid().ToString();

            try
            {
                userId = User.Claims.Where(c => c.Type.Equals(USR_ID)).FirstOrDefault().Value;
            }
            catch
            {
                userId = Guid.NewGuid().ToString();

            }
            var userIdParce = Guid.Parse(userId);
            return await _postulacionBussineLogic.AprovedVerificateNovedadesFs(verificate, userIdParce);
        }
    
    }
}
