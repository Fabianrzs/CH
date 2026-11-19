using GA2.Domain.Core.ReferenciasSvc.Fenix;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Haberes
{
    /// <summary>
    /// Consumo Haberes
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>30/06/2021</date>
    public class ConsumoHaberes : IConsumoHaberes
    {
        /// <summary>
        /// Instancias de configuracion
        /// </summary>
        private readonly ConfiguracionesSvc _configuration;
        /// <summary>
        /// Instancia de endpoint direccion
        /// </summary>
        private readonly EndpointAddress endpointAddress;
        /// <summary>
        /// Tipo de binding realizado al servicio
        /// </summary>
        public readonly BasicHttpBinding basicHttpBinding;

        /// <summary>
        /// logger de resgistro del consumo
        /// </summary>
        private readonly ILogger<ConsumoHaberes> _logger;

        

        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public ConsumoHaberes(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoHaberes> logger)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.HaberesUrlSvc);
            basicHttpBinding = new BasicHttpBinding(endpointAddress.Uri.Scheme.ToLower() == "http" ?
            BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport)
            {
                OpenTimeout = TimeSpan.MaxValue,
                CloseTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = 1_000_000
            };

            _logger = logger;
            
        }

        /// <summary>
        /// Obtener la instancia de conexion workmanager
        /// </summary>
        /// <returns>Instancia de servicio workmanager</returns>
        private wsModeloCanonicoDatosSoapClient ObtenerInstanciaCliente() =>
            new wsModeloCanonicoDatosSoapClient(basicHttpBinding, endpointAddress);

        public async Task<string> GenerarReporteHaberesV2(GenerarReporteV2Request reporteHaberes)
        {
            /// creacion de instancia consumo
            using var _haberes = this.ObtenerInstanciaCliente();
            await _haberes.OpenAsync();
            _logger.LogInformation(_haberes.State.ToString());

            string reportResult = string.Empty;

            // Deberia ser asi
            var reporte2 = await _haberes.GenerarReporteV2Async(reporteHaberes);

            

            if (reporte2.GenerarReporteV2Result.Status)
            {
                reportResult = Convert.ToBase64String(reporte2.GenerarReporteV2Result.Report);
            }

            return reportResult;/*await _haberes.GenerarReporteAsync(reporteHaberes.Reporte, reporteHaberes.ClienteIdentificacion, reporteHaberes.CuentaId);*/
        }

        public async Task<string> GenerarReporteHaberes(GenerarReporteRequest reporteHaberes)
        {
            /// creacion de instancia consumo
            using var _haberes = this.ObtenerInstanciaCliente();
            await _haberes.OpenAsync();
            _logger.LogInformation(_haberes.State.ToString());

            string reportResult = default;

            // Deberia ser asi
            var reporte2 = await _haberes.GenerarReporteAsync(reporteHaberes);

            

            if (reporte2.GenerarReporteResult.Status)
            {
                reportResult = Convert.ToBase64String(reporte2.GenerarReporteResult.Report);
            }

            return reportResult;/*await _haberes.GenerarReporteAsync(reporteHaberes.Reporte, reporteHaberes.ClienteIdentificacion, reporteHaberes.CuentaId);*/
        }

        public async Task<ReporteHaberesResponse> ReporteHaberes(ReporteHaberesRequest reporteHaberes)
        {
            /// creacion de instancia consumo
            using var _haberes = this.ObtenerInstanciaCliente();
            await _haberes.OpenAsync();
            _logger.LogInformation(_haberes.State.ToString());

            // Deberia ser asi
            var reporte2 = await _haberes.ReporteHaberesAsync(reporteHaberes);

            return reporte2;/*await _haberes.GenerarReporteAsync(reporteHaberes.Reporte, reporteHaberes.ClienteIdentificacion, reporteHaberes.CuentaId);*/
        }
    }
}
