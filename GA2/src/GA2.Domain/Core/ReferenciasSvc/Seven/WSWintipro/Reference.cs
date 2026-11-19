namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://seven/", ConfigurationName = "SINTIPROSoap")]
    public interface SINTIPROSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/GetCuentaContable", ReplyAction = "*")]
        GetCuentaContableResponse GetCuentaContable(GetCuentaContableRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/GetCuentaContable", ReplyAction = "*")]
        System.Threading.Tasks.Task<GetCuentaContableResponse> GetCuentaContableAsync(GetCuentaContableRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/GetCuentaContableTO", ReplyAction = "*")]
        GetCuentaContableTOResponse GetCuentaContableTO(GetCuentaContableTORequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/GetCuentaContableTO", ReplyAction = "*")]
        System.Threading.Tasks.Task<GetCuentaContableTOResponse> GetCuentaContableTOAsync(GetCuentaContableTORequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetCuentaContableRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "GetCuentaContable", Namespace = "http://seven/", Order = 0)]
        public GetCuentaContableRequestBody Body;

        public GetCuentaContableRequest()
        {
        }

        public GetCuentaContableRequest(GetCuentaContableRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class GetCuentaContableRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public int emp_codi;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string pro_codi;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string dsp_codi;

        public GetCuentaContableRequestBody()
        {
        }

        public GetCuentaContableRequestBody(int emp_codi, string pro_codi, string dsp_codi)
        {
            this.emp_codi = emp_codi;
            this.pro_codi = pro_codi;
            this.dsp_codi = dsp_codi;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetCuentaContableResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "GetCuentaContableResponse", Namespace = "http://seven/", Order = 0)]
        public GetCuentaContableResponseBody Body;

        public GetCuentaContableResponse()
        {
        }

        public GetCuentaContableResponse(GetCuentaContableResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class GetCuentaContableResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string GetCuentaContableResult;

        public GetCuentaContableResponseBody()
        {
        }

        public GetCuentaContableResponseBody(string GetCuentaContableResult)
        {
            this.GetCuentaContableResult = GetCuentaContableResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetCuentaContableTORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "GetCuentaContableTO", Namespace = "http://seven/", Order = 0)]
        public GetCuentaContableTORequestBody Body;

        public GetCuentaContableTORequest()
        {
        }

        public GetCuentaContableTORequest(GetCuentaContableTORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class GetCuentaContableTORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public int emp_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public int top_codi;

        public GetCuentaContableTORequestBody()
        {
        }

        public GetCuentaContableTORequestBody(int emp_codi, int top_codi)
        {
            this.emp_codi = emp_codi;
            this.top_codi = top_codi;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetCuentaContableTOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "GetCuentaContableTOResponse", Namespace = "http://seven/", Order = 0)]
        public GetCuentaContableTOResponseBody Body;

        public GetCuentaContableTOResponse()
        {
        }

        public GetCuentaContableTOResponse(GetCuentaContableTOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class GetCuentaContableTOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string GetCuentaContableTOResult;

        public GetCuentaContableTOResponseBody()
        {
        }

        public GetCuentaContableTOResponseBody(string GetCuentaContableTOResult)
        {
            this.GetCuentaContableTOResult = GetCuentaContableTOResult;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface SINTIPROSoapChannel : SINTIPROSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class SINTIPROSoapClient : System.ServiceModel.ClientBase<SINTIPROSoap>, SINTIPROSoap
    {

        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public SINTIPROSoapClient(EndpointConfiguration endpointConfiguration) :
                base(SINTIPROSoapClient.GetBindingForEndpoint(endpointConfiguration), SINTIPROSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SINTIPROSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(SINTIPROSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SINTIPROSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(SINTIPROSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SINTIPROSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public GetCuentaContableResponse GetCuentaContable(GetCuentaContableRequest request)
        {
            return base.Channel.GetCuentaContable(request);
        }

        public System.Threading.Tasks.Task<GetCuentaContableResponse> GetCuentaContableAsync(GetCuentaContableRequest request)
        {
            return base.Channel.GetCuentaContableAsync(request);
        }

        public GetCuentaContableTOResponse GetCuentaContableTO(GetCuentaContableTORequest request)
        {
            return base.Channel.GetCuentaContableTO(request);
        }

        public System.Threading.Tasks.Task<GetCuentaContableTOResponse> GetCuentaContableTOAsync(GetCuentaContableTORequest request)
        {
            return base.Channel.GetCuentaContableTOAsync(request);
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
            if ((endpointConfiguration == EndpointConfiguration.SINTIPROSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.SINTIPROSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.SINTIPROSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/Wintipro/WInTipro.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.SINTIPROSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/Wintipro/WInTipro.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            SINTIPROSoap,

            SINTIPROSoap12,
        }
    }
}
