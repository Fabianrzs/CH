using GA2.Application.Dto;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTercero
{
    /// <summary>
    /// Consumidor de vigia verficicion tercero
    /// </summary>
    public class ConsumoVigiaCargaTercero : IConsumoVigiaCargaTercero
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
        private readonly SOAP_CargaTerceroSoapClient _cargaTercero;
        /// <summary>
        /// logger de resgistro del consumo
        /// </summary>
        private readonly ILogger<ConsumoVigiaCargaTercero> _logger;

        /// <summary>
        /// Construnctor consumindor
        /// </summary>
        /// <param name="configurationes"></param>
        /// <param name="logger"></param>
        public ConsumoVigiaCargaTercero(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoVigiaCargaTercero> logger)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.VigiaCargaTerceroUrlSvc);
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
            _cargaTercero = this.ObtenerInstanciaCliente();
        }

        /// <summary>
        /// Obtener la instancia de conexion carga tercero
        /// </summary>
        /// <returns>Instancia de servicio carga</returns>
        private SOAP_CargaTerceroSoapClient ObtenerInstanciaCliente() => new SOAP_CargaTerceroSoapClient(basicHttpBinding, endpointAddress);

        public async Task<Resultado> CargaTerceroAsync(CargaTerceroRequest cargaTerceroRequest)
        {
            _logger.LogInformation(_cargaTercero.State.ToString());
            return await _cargaTercero.F_CARGATERCEROAsync(
                    cargaTerceroRequest.origen,
                    cargaTerceroRequest.tipide_codigo,
                    cargaTerceroRequest.numide,
                    cargaTerceroRequest.nombres,
                    cargaTerceroRequest.apellidos,
                    cargaTerceroRequest.sisgeo_codigo,
                    cargaTerceroRequest.tipo,
                    cargaTerceroRequest.profesion_codigo,
                    cargaTerceroRequest.actividad_codigo,
                    cargaTerceroRequest.activo,
                    cargaTerceroRequest.pasivo,
                    cargaTerceroRequest.patrimonio,
                    cargaTerceroRequest.ingresos,
                    cargaTerceroRequest.egresos,
                    cargaTerceroRequest.cupototal,
                    cargaTerceroRequest.tipcal_codigo,
                    cargaTerceroRequest.tipvin_codigo,
                    cargaTerceroRequest.direccion,
                    cargaTerceroRequest.telefono,
                    cargaTerceroRequest.numcelular,
                    cargaTerceroRequest.email,
                    cargaTerceroRequest.fecha_vinculacion,
                    cargaTerceroRequest.fecha_actualizacion,
                    cargaTerceroRequest.funcionario_responsable,
                    cargaTerceroRequest.ciiu_codigo,
                    cargaTerceroRequest.pep,
                    cargaTerceroRequest.tippep_codigo,
                    cargaTerceroRequest.sucursal_codigo,
                    cargaTerceroRequest.total_otros_ingresos,
                    cargaTerceroRequest.fecha_nacimiento,
                    cargaTerceroRequest.exentoanalisis,
                    cargaTerceroRequest.tipide_referencia,
                    cargaTerceroRequest.numide_referencia,
                    cargaTerceroRequest.tipoderelacion_codigo,
                    cargaTerceroRequest.sexo,
                    cargaTerceroRequest.comentarios,
                    cargaTerceroRequest.detotring,
                    cargaTerceroRequest.autorizanotificacion,
                    cargaTerceroRequest.dv,
                    cargaTerceroRequest.naturalezajuridica,
                    cargaTerceroRequest.operamonedaextranjera,
                    cargaTerceroRequest.manejarecursospublicos,
                    cargaTerceroRequest.sisgeo_nacionalidad,
                    cargaTerceroRequest.estado,
                    cargaTerceroRequest.usuadi);
        }
    }
}
