namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://seven/", ConfigurationName = "ValidarAportesSoap")]
    public interface ValidarAportesSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/GetAporte", ReplyAction = "*")]
        GetAporteResponse GetAporte(GetAporteRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/GetAporte", ReplyAction = "*")]
        System.Threading.Tasks.Task<GetAporteResponse> GetAporteAsync(GetAporteRequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetAporteRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "GetAporte", Namespace = "http://seven/", Order = 0)]
        public GetAporteRequestBody Body;

        public GetAporteRequest()
        {
        }

        public GetAporteRequest(GetAporteRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class GetAporteRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public int emp_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public int top_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 2)]
        public int mte_nume;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public int mte_anop;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string ter_coda;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string arb_coda;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 6)]
        public int cie_codi;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string ind_clas;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string ind_tipo;

        public GetAporteRequestBody()
        {
        }

        public GetAporteRequestBody(int emp_codi, int top_codi, int mte_nume, int mte_anop, string ter_coda, string arb_coda, int cie_codi, string ind_clas, string ind_tipo)
        {
            this.emp_codi = emp_codi;
            this.top_codi = top_codi;
            this.mte_nume = mte_nume;
            this.mte_anop = mte_anop;
            this.ter_coda = ter_coda;
            this.arb_coda = arb_coda;
            this.cie_codi = cie_codi;
            this.ind_clas = ind_clas;
            this.ind_tipo = ind_tipo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetAporteResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "GetAporteResponse", Namespace = "http://seven/", Order = 0)]
        public GetAporteResponseBody Body;

        public GetAporteResponse()
        {
        }

        public GetAporteResponse(GetAporteResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class GetAporteResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string GetAporteResult;

        public GetAporteResponseBody()
        {
        }

        public GetAporteResponseBody(string GetAporteResult)
        {
            this.GetAporteResult = GetAporteResult;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface ValidarAportesSoapChannel : ValidarAportesSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class ValidarAportesSoapClient : System.ServiceModel.ClientBase<ValidarAportesSoap>, ValidarAportesSoap
    {

        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public ValidarAportesSoapClient(EndpointConfiguration endpointConfiguration) :
                base(ValidarAportesSoapClient.GetBindingForEndpoint(endpointConfiguration), ValidarAportesSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ValidarAportesSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(ValidarAportesSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ValidarAportesSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(ValidarAportesSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public ValidarAportesSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public GetAporteResponse GetAporte(GetAporteRequest request)
        {
            return base.Channel.GetAporte(request);
        }

        public System.Threading.Tasks.Task<GetAporteResponse> GetAporteAsync(GetAporteRequest request)
        {
            return base.Channel.GetAporteAsync(request);
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
            if ((endpointConfiguration == EndpointConfiguration.ValidarAportesSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ValidarAportesSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.ValidarAportesSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSAportes/ValidarAportes.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.ValidarAportesSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSAportes/ValidarAportes.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            ValidarAportesSoap,

            ValidarAportesSoap12,
        }
    }
}
