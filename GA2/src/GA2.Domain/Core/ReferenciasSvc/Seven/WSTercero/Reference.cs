namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://seven/", ConfigurationName = "SGNTERCESoap")]
    public interface SGNTERCESoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarGnTerce", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        InsertarGnTerceResponse InsertarGnTerce(InsertarGnTerceRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarGnTerce", ReplyAction = "*")]
        System.Threading.Tasks.Task<InsertarGnTerceResponse> InsertarGnTerceAsync(InsertarGnTerceRequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "InsertarGnTerce", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class InsertarGnTerceRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public int tip_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public string ter_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public int ter_dive;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 4)]
        public string ter_nomb;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 5)]
        public string ter_apel;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 6)]
        public string ter_nocm;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 7)]
        public int mod_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 8)]
        public int pai_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 9)]
        public int dep_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 10)]
        public int mun_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 11)]
        public string ter_dire;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 12)]
        public string ter_ntel;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 13)]
        public string ter_mail;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 14)]
        public string ter_nfax;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 15)]
        public string arb_clte;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 16)]
        public string ter_audp;

        public InsertarGnTerceRequest()
        {
        }

        public InsertarGnTerceRequest(
                    int emp_codi,
                    int tip_codi,
                    string ter_coda,
                    int ter_dive,
                    string ter_nomb,
                    string ter_apel,
                    string ter_nocm,
                    int mod_codi,
                    int pai_codi,
                    int dep_codi,
                    int mun_codi,
                    string ter_dire,
                    string ter_ntel,
                    string ter_mail,
                    string ter_nfax,
                    string arb_clte,
                    string ter_audp)
        {
            this.emp_codi = emp_codi;
            this.tip_codi = tip_codi;
            this.ter_coda = ter_coda;
            this.ter_dive = ter_dive;
            this.ter_nomb = ter_nomb;
            this.ter_apel = ter_apel;
            this.ter_nocm = ter_nocm;
            this.mod_codi = mod_codi;
            this.pai_codi = pai_codi;
            this.dep_codi = dep_codi;
            this.mun_codi = mun_codi;
            this.ter_dire = ter_dire;
            this.ter_ntel = ter_ntel;
            this.ter_mail = ter_mail;
            this.ter_nfax = ter_nfax;
            this.arb_clte = arb_clte;
            this.ter_audp = ter_audp;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "InsertarGnTerceResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class InsertarGnTerceResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string InsertarGnTerceResult;

        public InsertarGnTerceResponse()
        {
        }

        public InsertarGnTerceResponse(string InsertarGnTerceResult)
        {
            this.InsertarGnTerceResult = InsertarGnTerceResult;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface SGNTERCESoapChannel : SGNTERCESoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class SGNTERCESoapClient : System.ServiceModel.ClientBase<SGNTERCESoap>, SGNTERCESoap
    {

        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public SGNTERCESoapClient(EndpointConfiguration endpointConfiguration) :
                base(SGNTERCESoapClient.GetBindingForEndpoint(endpointConfiguration), SGNTERCESoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SGNTERCESoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(SGNTERCESoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SGNTERCESoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(SGNTERCESoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SGNTERCESoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public InsertarGnTerceResponse InsertarGnTerce(InsertarGnTerceRequest request)
        {
            return base.Channel.InsertarGnTerce(request);
        }

        public System.Threading.Tasks.Task<InsertarGnTerceResponse> InsertarGnTerceAsync(InsertarGnTerceRequest request)
        {
            return base.Channel.InsertarGnTerceAsync(request);
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
            if ((endpointConfiguration == EndpointConfiguration.SGNTERCESoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.SGNTERCESoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.SGNTERCESoap))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSTercero/WGnTerce.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.SGNTERCESoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSTercero/WGnTerce.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            SGNTERCESoap,

            SGNTERCESoap12,
        }
    }
}
