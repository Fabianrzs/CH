using GA2.Domain.Core.ReferenciasSvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Dto;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Models;

namespace GA2.Apis.GA2.Endpoints.Integraciones
{
    [ApiController]
    [Route("api/integrations/tusdatos")]
    public class TusDatosController : ControllerBase
    {
        private readonly ITusDatosClient _client;
        private readonly ILogger<TusDatosController> _logger;

        public TusDatosController(
            ITusDatosClient client,
            ILogger<TusDatosController> logger)
        {
            _client = client;
            _logger = logger;
        }

        /// <summary>
        /// Consulta información del cliente en TusDatos.
        /// </summary>
        /// <param name="identification"></param>
        /// <param name="documentType"></param>
        /// <param name="checkHistory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet(
            "customer/{identification}/document-type/{documentType}")]
        [ProducesResponseType(
            typeof(DatosClienteResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarAsync(
            [FromRoute] string identification,
            [FromRoute] string documentType,
            [FromQuery] bool checkHistory = true,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Consultando TusDatos para identificación {Identification}",
                identification);

            var response = await _client.ConsultarAsync(
                identification,
                documentType,
                checkHistory,
                cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Consulta riesgo LAFT en Vigia.
        /// </summary>
        /// <param name="identification"></param>
        /// <param name="documentType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet(
            "vigia/customer/{identification}/document-type/{documentType}")]
        [ProducesResponseType(
            typeof(VigiaResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarVigiaAsync(
            [FromRoute] string identification,
            [FromRoute] string documentType,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Consultando riesgo LAFT en Vigia para identificación {Identification}",
                identification);

            var response = await _client.ConsultarVigiaAsync(
                identification,
                documentType,
                cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Consulta el estado de un job en TusDatos.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="identification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet(
            "job/{jobId}/customer/{identification}")]
        [ProducesResponseType(
            typeof(DatosClienteJobResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarJobAsync(
            [FromRoute] string jobId,
            [FromRoute] string identification,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Consultando estado job TusDatos {JobId}",
                jobId);

            var response = await _client.ConsultarJobAsync(
                jobId,
                identification,
                cancellationToken);

            return Ok(response);
        }
    }
}
