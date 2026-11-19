using GA2.Application.Dto;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTransaccion
{
    /// <summary>
    /// Consumidor de vigia verficicion tercero
    /// </summary>
    public class ConsumoVigiaCargaTransaccion : IConsumoVigiaCargaTransaccion
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
        private readonly SOAP_CargaTransaccionSoapClient _cargaTransaccion;
        /// <summary>
        /// logger de resgistro del consumo
        /// </summary>
        private readonly ILogger<ConsumoVigiaCargaTransaccion> _logger;

        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public ConsumoVigiaCargaTransaccion(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoVigiaCargaTransaccion> logger)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.VigiaCargaTransaccionUrlSvc);
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
            _cargaTransaccion = this.ObtenerInstanciaCliente();
        }

        /// <summary>
        /// Obtener la instancia de conexion carga transaccion
        /// </summary>
        /// <returns>Instancia de servicio carga</returns>
        private SOAP_CargaTransaccionSoapClient ObtenerInstanciaCliente() => new SOAP_CargaTransaccionSoapClient(basicHttpBinding, endpointAddress);

        /// <summary>
        /// CargarTransaction 
        /// </summary>
        /// <param name="cargaTransaccionRequest"></param>
        /// <returns></returns>
        public async Task<Resultado> CargaTransaccionAsync(CargaTransaccionRequest cargaTransaccionRequest)
        {
            _logger.LogInformation(_cargaTransaccion.State.ToString());
            return await _cargaTransaccion.F_CARGATRANSACCIONAsync(
                                                                   cargaTransaccionRequest.origen,
                                                                   cargaTransaccionRequest.tercero_tipide_codigo,
                                                                   cargaTransaccionRequest.tercero_numide,
                                                                   cargaTransaccionRequest.producto_codigo,
                                                                   cargaTransaccionRequest.producto_numero,
                                                                   cargaTransaccionRequest.tiptran_codigo,
                                                                   cargaTransaccionRequest.fecha,
                                                                   cargaTransaccionRequest.consecutivo,
                                                                   cargaTransaccionRequest.sisgeo_codigo,
                                                                   cargaTransaccionRequest.sucursal_codigo,
                                                                   cargaTransaccionRequest.valor,
                                                                   cargaTransaccionRequest.valormonedaextranjera,
                                                                   cargaTransaccionRequest.tipmon_codigo,
                                                                   cargaTransaccionRequest.intermediario_codigo,
                                                                   cargaTransaccionRequest.tipide_remitente,
                                                                   cargaTransaccionRequest.numide_remitente,
                                                                   cargaTransaccionRequest.nombreremitente,
                                                                   cargaTransaccionRequest.apellidoremitente,
                                                                   cargaTransaccionRequest.codigopaisremite,
                                                                   cargaTransaccionRequest.ciudadremite,
                                                                   cargaTransaccionRequest.direccionremite,
                                                                   cargaTransaccionRequest.telefonoremite,
                                                                   cargaTransaccionRequest.estadooperacion,
                                                                   cargaTransaccionRequest.ciudadcliente,
                                                                   cargaTransaccionRequest.direccioncliente,
                                                                   cargaTransaccionRequest.telefonocliente,
                                                                   cargaTransaccionRequest.medio_codigo,
                                                                   cargaTransaccionRequest.formapago_codigo,
                                                                   cargaTransaccionRequest.referencia1,
                                                                   cargaTransaccionRequest.referencia2,
                                                                   cargaTransaccionRequest.observaciones,
                                                                   cargaTransaccionRequest.dispositivo_codigo,
                                                                   cargaTransaccionRequest.direcciontransaccion,
                                                                   cargaTransaccionRequest.usuadi);
        }
    }
}
