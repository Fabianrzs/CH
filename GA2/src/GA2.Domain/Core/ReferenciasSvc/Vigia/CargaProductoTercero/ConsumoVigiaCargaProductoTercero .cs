using GA2.Application.Dto;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Vigia.CargaProductoTercero
{
    /// <summary>
    /// Consumidor de vigia verficicion tercero
    /// </summary>
    public class ConsumoVigiaCargaProductoTercero : IConsumoVigiaCargaProductoTercero
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
        /// Cliente de conexion a verficacion
        /// </summary>
        private readonly SOAP_CargaProductosTerceroSoapClient _cargaProductoTercero;
        /// <summary>
        /// logger de resgistro del consumo
        /// </summary>
        private readonly ILogger<ConsumoVigiaCargaProductoTercero> _logger;

        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public ConsumoVigiaCargaProductoTercero(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoVigiaCargaProductoTercero> logger)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.VigiaCargaProductosTerceroUrlSvc);
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

            /// creacion de instancia consumo
            _cargaProductoTercero = this.ObtenerInstanciaCliente();
        }

        /// <summary>
        /// Obtener la instancia de conexion carga producto tercero
        /// </summary>
        /// <returns>Instancia de servicio carga</returns>
        private SOAP_CargaProductosTerceroSoapClient ObtenerInstanciaCliente() => new SOAP_CargaProductosTerceroSoapClient(basicHttpBinding, endpointAddress);


        public async Task<Resultado> CargaProductoTerceroAsync(CargaProductoTerceroRequest cargaProductoTerceroRequest)
        {
            _logger.LogInformation(_cargaProductoTercero.State.ToString());
            return await _cargaProductoTercero.F_CARGAPRODUCTOTERCEROAsync(
                cargaProductoTerceroRequest.Origen,
                cargaProductoTerceroRequest.Tipide_codigo,
                cargaProductoTerceroRequest.Numide,
                cargaProductoTerceroRequest.Producto_codigo,
                cargaProductoTerceroRequest.Producto_numero,
                cargaProductoTerceroRequest.Sucursal_codigo,
                cargaProductoTerceroRequest.Fechaapertura,
                cargaProductoTerceroRequest.Cupo,
                cargaProductoTerceroRequest.Saldo,
                cargaProductoTerceroRequest.Cuota,
                cargaProductoTerceroRequest.Vigencia,
                cargaProductoTerceroRequest.Titularidad,
                cargaProductoTerceroRequest.Fechacancelacion);
        }
    }
}
