using GA2.Domain.Core.ReferenciasSvc.Fenix;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Fenix
{
    /// <summary>
    /// Servicio fenix
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>05/08/2021</date>
    public class FenixService : IFenixService
    {
        /// <summary>
        /// Referencia servicio consumo
        /// </summary>
        private readonly IConsumoFenix _consumoFenix;
        /// <summary>
        /// logger application 
        /// </summary>
        private readonly ILogger<FenixService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consumoFenix">Service consumo</param>
        /// <param name="logger">logger application</param>
        public FenixService(IConsumoFenix consumoFenix, ILogger<FenixService> logger)
        {
            _consumoFenix = consumoFenix;
            _logger = logger;
        }

        /// <summary>
        /// Validacion Biometrico 
        /// </summary>
        /// <param name="identification">Identificacion persona </param>
        /// <returns>Xml referencia de servico</returns>
        public async Task<ValidarBiometriaResponse> ValidarBiometriaAsync(ValidarBiometriaRequest identification)
        {
            _logger.LogInformation($"Consultando identificacion: {identification}");
             return await _consumoFenix.ValidarBiometriaAsync(identification);
        }
    }
}
