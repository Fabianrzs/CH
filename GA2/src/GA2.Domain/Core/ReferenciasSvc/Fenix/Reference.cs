namespace GA2.Domain.Core.ReferenciasSvc.Fenix
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "Fenix.WebServicesBiometriasSoap")]
    public interface WebServicesBiometriasSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/xxValidarBiometria", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        Fenix.xxValidarBiometriaResponse xxValidarBiometria(Fenix.xxValidarBiometriaRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/xxValidarBiometria", ReplyAction = "*")]
        System.Threading.Tasks.Task<Fenix.xxValidarBiometriaResponse> xxValidarBiometriaAsync(Fenix.xxValidarBiometriaRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ValidarBiometria", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        Fenix.ValidarBiometriaResponse ValidarBiometria(Fenix.ValidarBiometriaRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ValidarBiometria", ReplyAction = "*")]
        System.Threading.Tasks.Task<Fenix.ValidarBiometriaResponse> ValidarBiometriaAsync(Fenix.ValidarBiometriaRequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "xxValidarBiometria", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
    public partial class xxValidarBiometriaRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int identificacion;

        public xxValidarBiometriaRequest()
        {
        }

        public xxValidarBiometriaRequest(int identificacion)
        {
            this.identificacion = identificacion;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "xxValidarBiometriaResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
    public partial class xxValidarBiometriaResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public Fenix.ArrayOfXElement xxValidarBiometriaResult;

        public xxValidarBiometriaResponse()
        {
        }

        public xxValidarBiometriaResponse(Fenix.ArrayOfXElement xxValidarBiometriaResult)
        {
            this.xxValidarBiometriaResult = xxValidarBiometriaResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ValidarBiometria", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
    public partial class ValidarBiometriaRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int identificacion;

        public ValidarBiometriaRequest()
        {
        }

        public ValidarBiometriaRequest(int identificacion)
        {
            this.identificacion = identificacion;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ValidarBiometriaResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
    public partial class ValidarBiometriaResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public Fenix.ArrayOfXElement ValidarBiometriaResult;

        public ValidarBiometriaResponse()
        {
        }

        public ValidarBiometriaResponse(Fenix.ArrayOfXElement ValidarBiometriaResult)
        {
            this.ValidarBiometriaResult = ValidarBiometriaResult;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    public interface WebServicesBiometriasSoapChannel : Fenix.WebServicesBiometriasSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview2.21264.1")]
    public partial class WebServicesBiometriasSoapClient : System.ServiceModel.ClientBase<Fenix.WebServicesBiometriasSoap>, Fenix.WebServicesBiometriasSoap
    {

        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public WebServicesBiometriasSoapClient(EndpointConfiguration endpointConfiguration) :
                base(WebServicesBiometriasSoapClient.GetBindingForEndpoint(endpointConfiguration), WebServicesBiometriasSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public WebServicesBiometriasSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(WebServicesBiometriasSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public WebServicesBiometriasSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(WebServicesBiometriasSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public WebServicesBiometriasSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public Fenix.xxValidarBiometriaResponse xxValidarBiometria(Fenix.xxValidarBiometriaRequest request)
        {
            return base.Channel.xxValidarBiometria(request);
        }

        public System.Threading.Tasks.Task<Fenix.xxValidarBiometriaResponse> xxValidarBiometriaAsync(Fenix.xxValidarBiometriaRequest request)
        {
            return base.Channel.xxValidarBiometriaAsync(request);
        }

        public Fenix.ValidarBiometriaResponse ValidarBiometria(Fenix.ValidarBiometriaRequest request)
        {
            return base.Channel.ValidarBiometria(request);
        }

        public System.Threading.Tasks.Task<Fenix.ValidarBiometriaResponse> ValidarBiometriaAsync(Fenix.ValidarBiometriaRequest request)
        {
            return base.Channel.ValidarBiometriaAsync(request);
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
            if ((endpointConfiguration == EndpointConfiguration.WebServicesBiometriasSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.WebServicesBiometriasSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.WebServicesBiometriasSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-09xc/WsFenixBiometria/WebServicesBiometrias.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.WebServicesBiometriasSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-09xc/WsFenixBiometria/WebServicesBiometrias.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            WebServicesBiometriasSoap,

            WebServicesBiometriasSoap12,
        }
    }

    [System.Xml.Serialization.XmlSchemaProviderAttribute(null, IsAny = true)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil-lib", "2.0.3.0")]
    public partial class ArrayOfXElement : object, System.Xml.Serialization.IXmlSerializable
    {

        private System.Collections.Generic.List<System.Xml.Linq.XElement> nodesList = new System.Collections.Generic.List<System.Xml.Linq.XElement>();

        public ArrayOfXElement()
        {
        }

        public virtual System.Collections.Generic.List<System.Xml.Linq.XElement> Nodes
        {
            get
            {
                return this.nodesList;
            }
        }

        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new System.NotImplementedException();
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            System.Collections.Generic.IEnumerator<System.Xml.Linq.XElement> e = nodesList.GetEnumerator();
            for (
            ; e.MoveNext();
            )
            {
                ((System.Xml.Serialization.IXmlSerializable)(e.Current)).WriteXml(writer);
            }
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            for (
            ; (reader.NodeType != System.Xml.XmlNodeType.EndElement);
            )
            {
                if ((reader.NodeType == System.Xml.XmlNodeType.Element))
                {
                    System.Xml.Linq.XElement elem = new System.Xml.Linq.XElement("default");
                    ((System.Xml.Serialization.IXmlSerializable)(elem)).ReadXml(reader);
                    Nodes.Add(elem);
                }
                else
                {
                    reader.Skip();
                }
            }
        }
    }
}
