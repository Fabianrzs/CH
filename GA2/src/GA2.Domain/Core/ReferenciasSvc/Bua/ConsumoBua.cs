using GA2.Domain.Core.BuaServiceSvc;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Bua
{
    public class ConsumoBua : IConsumoBua
    {
        private readonly ConfiguracionesSvc _configuration;
        private readonly EndpointAddress endpointAddress;
        public readonly BasicHttpBinding basicHttpBinding;
        


        public ConsumoBua(IOptions<ConfiguracionesSvc> configurationes)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.BuaUrlSvc);
            basicHttpBinding = new BasicHttpBinding(endpointAddress.Uri.Scheme.ToLower() == "https" ?
            BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport)
            {
                OpenTimeout = TimeSpan.MaxValue,
                CloseTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue
            };
            
        }

        public BUA_APIClient ObtenerInstanciaCliente()
        {
            return new BUA_APIClient();
        }

        public async Task<BUA> ObtenerBua(BUAConsultaBE consulta)
        {
            using var _bua = this.ObtenerInstanciaCliente();
            await _bua.OpenAsync();
            Console.WriteLine(_bua.State);

            var response = await _bua.ObtenerBUAAsync(consulta);

            await _bua.CloseAsync();
            return response;
        }
    }
}
