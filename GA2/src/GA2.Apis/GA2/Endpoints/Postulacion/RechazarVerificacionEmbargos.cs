using Ardalis.ApiEndpoints;
using GA2.Domain.Core.Postulacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using System.Linq;
using GA2.Application.Dto.Postulacion;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class RechazarVerificacionEmbargos : BaseAsyncEndpoint.WithRequest<VeficateFondoDto>
        .WithResponse<Response<string>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        private const string USR_ID = "USR_ID";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bussigneLogic"></param>
        public RechazarVerificacionEmbargos(IPostulacionBussineLogic bussigneLogic)
        {
            _postulacionBussineLogic = bussigneLogic;
        }
        [HttpPost("api/postulante/RechazarVerificateEmbargosPostulacion")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "rechazar verificacion embargo de postulacion",
            Description = "rechazar la verificacion embargos de postulacion",
            OperationId = "postulante.RechazarVerificateEmbargosPostulacion",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(VeficateFondoDto request, CancellationToken cancellationToken = default)
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
            return await _postulacionBussineLogic.RechazarVerificateEmbargosPostulacion(request, userIdParce);
        }
    }
}
