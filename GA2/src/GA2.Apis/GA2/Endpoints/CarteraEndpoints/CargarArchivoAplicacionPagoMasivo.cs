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
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class CargarArchivoAplicacionPagoMasivo : BaseAsyncEndpoint
        .WithRequest<IFormFile>
        .WithResponse<Response<List<ResultadoAplicacionPagoDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public CargarArchivoAplicacionPagoMasivo(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic ?? throw new ArgumentException(nameof(carteraBusinessLogic));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/CargarArchivoAplicacionPagoMasivo/{fechaAplicacion}&{valorPago}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Response<List<AplicacionPagoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Cargar archivo aplicacion pago masivo",
            Description = "Cargar archivo aplicacion pago masivo",
            OperationId = "cartera.CargarArchivoAplicacionPagoMasivo",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<List<ResultadoAplicacionPagoDto>>>> HandleAsync(IFormFile request, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            if (string.IsNullOrWhiteSpace(userId)) throw new InvalidOperationException(nameof(userId));
            ParametrosAplicacionPagoDto parametros = new ParametrosAplicacionPagoDto()
            {
                FechaAplicacion = DateTime.Parse(Request.Path.Value.Split("/")[4].Split("&")[0].Replace("%2F", "/")),
                ValorPago = decimal.Parse(Request.Path.Value.Split("/")[4].Split("&")[1], CultureInfo.InvariantCulture),
                FormatoAplicar = int.Parse(Request.Path.Value.Split("/")[4].Split("&")[2]),
                AplicanPagos = bool.Parse(Request.Path.Value.Split("/")[4].Split("&")[3]),
                CreadoPor = Guid.Parse(userId)
            };

            return await _carteraBusinessLogic.CargarArchivoPagoMasivo(request, parametros);
            
        }
    }
}
