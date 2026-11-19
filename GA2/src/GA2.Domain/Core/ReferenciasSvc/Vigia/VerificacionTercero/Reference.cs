namespace GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "VigiaServiceSvc.SOAP_VerificaTerceroSoap")]
    public interface SOAP_VerificaTerceroSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/F_VERIFICATERCEROV3", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero.Resultado F_VERIFICATERCEROV3(string pe_origen, string pe_dato, string pe_tipoverificacion, string pe_porcentaje);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/F_VERIFICATERCEROV3", ReplyAction = "*")]
        System.Threading.Tasks.Task<GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero.Resultado> F_VERIFICATERCEROV3Async(string pe_origen, string pe_dato, string pe_tipoverificacion, string pe_porcentaje);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class Resultado
    {

        private string encontradoField;

        private string metodoField;

        private string listasField;

        private string errorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Encontrado
        {
            get
            {
                return this.encontradoField;
            }
            set
            {
                this.encontradoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Metodo
        {
            get
            {
                return this.metodoField;
            }
            set
            {
                this.metodoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Listas
        {
            get
            {
                return this.listasField;
            }
            set
            {
                this.listasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    public interface SOAP_VerificaTerceroSoapChannel : GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero.SOAP_VerificaTerceroSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    public partial class SOAP_VerificaTerceroSoapClient : System.ServiceModel.ClientBase<GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero.SOAP_VerificaTerceroSoap>, 
        GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero.SOAP_VerificaTerceroSoap
    {

        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public SOAP_VerificaTerceroSoapClient(EndpointConfiguration endpointConfiguration) :
                base(SOAP_VerificaTerceroSoapClient.GetBindingForEndpoint(endpointConfiguration), SOAP_VerificaTerceroSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SOAP_VerificaTerceroSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(SOAP_VerificaTerceroSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SOAP_VerificaTerceroSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(SOAP_VerificaTerceroSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SOAP_VerificaTerceroSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero.Resultado F_VERIFICATERCEROV3(string pe_origen, string pe_dato, string pe_tipoverificacion, string pe_porcentaje)
        {
            return base.Channel.F_VERIFICATERCEROV3(pe_origen, pe_dato, pe_tipoverificacion, pe_porcentaje);
        }

        public System.Threading.Tasks.Task<GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero.Resultado> F_VERIFICATERCEROV3Async(string pe_origen, string pe_dato, string pe_tipoverificacion, string pe_porcentaje)
        {
            return base.Channel.F_VERIFICATERCEROV3Async(pe_origen, pe_dato, pe_tipoverificacion, pe_porcentaje);
        }

        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }

        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }

        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SOAP_VerificaTerceroSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.SOAP_VerificaTerceroSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SOAP_VerificaTerceroSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-22fw:8080/WebSvc_asmx/SOAP_VerificaTercero.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.SOAP_VerificaTerceroSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-22fw:8080/WebSvc_asmx/SOAP_VerificaTercero.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            SOAP_VerificaTerceroSoap,

            SOAP_VerificaTerceroSoap12,
        }
    }
}
