using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://seven/", ConfigurationName = "SPGOBLIGSoap")]
    public interface SPGOBLIGSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarCDP", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        InsertarCDPResponse InsertarCDP(InsertarCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarCDP", ReplyAction = "*")]
        System.Threading.Tasks.Task<InsertarCDPResponse> InsertarCDPAsync(InsertarCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarDetCDP", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        InsertarDetCDPResponse InsertarDetCDP(InsertarDetCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarDetCDP", ReplyAction = "*")]
        System.Threading.Tasks.Task<InsertarDetCDPResponse> InsertarDetCDPAsync(InsertarDetCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/AplicarCDP", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        AplicarCDPResponse AplicarCDP(AplicarCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/AplicarCDP", ReplyAction = "*")]
        System.Threading.Tasks.Task<AplicarCDPResponse> AplicarCDPAsync(AplicarCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/AnularCDP", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        AnularCDPResponse AnularCDP(AnularCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/AnularCDP", ReplyAction = "*")]
        System.Threading.Tasks.Task<AnularCDPResponse> AnularCDPAsync(AnularCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/CrearCDP", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        CrearCDPResponse CrearCDP(CrearCDPRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/CrearCDP", ReplyAction = "*")]
        System.Threading.Tasks.Task<CrearCDPResponse> CrearCDPAsync(CrearCDPRequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "InsertarCDP", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class InsertarCDPRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public int top_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public int mpr_nume;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public System.DateTime mpr_fech;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 4)]
        public string mpr_desc;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 5)]
        public string arb_cods;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 6)]
        public string ter_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 7)]
        public string mpr_tdos;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 8)]
        public int mpr_ndos;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 9)]
        public int uni_codi;

        public InsertarCDPRequest()
        {
        }

        public InsertarCDPRequest(int emp_codi, int top_codi, int mpr_nume, System.DateTime mpr_fech, string mpr_desc, string arb_cods, string ter_coda, string mpr_tdos, int mpr_ndos, int uni_codi)
        {
            this.emp_codi = emp_codi;
            this.top_codi = top_codi;
            this.mpr_nume = mpr_nume;
            this.mpr_fech = mpr_fech;
            this.mpr_desc = mpr_desc;
            this.arb_cods = arb_cods;
            this.ter_coda = ter_coda;
            this.mpr_tdos = mpr_tdos;
            this.mpr_ndos = mpr_ndos;
            this.uni_codi = uni_codi;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "InsertarCDPResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class InsertarCDPResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string InsertarCDPResult;

        public InsertarCDPResponse()
        {
        }

        public InsertarCDPResponse(string InsertarCDPResult)
        {
            this.InsertarCDPResult = InsertarCDPResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "InsertarDetCDP", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class InsertarDetCDPRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public int mpr_cont;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public string rub_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public string pro_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 4)]
        public string arb_codc;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 5)]
        public string arb_codp;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 6)]
        public string arb_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 7)]
        public double dmp_valo;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 8)]
        public string dmp_desc;

        public InsertarDetCDPRequest()
        {
        }

        public InsertarDetCDPRequest(int emp_codi, int mpr_cont, string rub_codi, string pro_codi, string arb_codc, string arb_codp, string arb_coda, double dmp_valo, string dmp_desc)
        {
            this.emp_codi = emp_codi;
            this.mpr_cont = mpr_cont;
            this.rub_codi = rub_codi;
            this.pro_codi = pro_codi;
            this.arb_codc = arb_codc;
            this.arb_codp = arb_codp;
            this.arb_coda = arb_coda;
            this.dmp_valo = dmp_valo;
            this.dmp_desc = dmp_desc;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "InsertarDetCDPResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class InsertarDetCDPResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string InsertarDetCDPResult;

        public InsertarDetCDPResponse()
        {
        }

        public InsertarDetCDPResponse(string InsertarDetCDPResult)
        {
            this.InsertarDetCDPResult = InsertarDetCDPResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AplicarCDP", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class AplicarCDPRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public int mpr_cont;

        public AplicarCDPRequest()
        {
        }

        public AplicarCDPRequest(int emp_codi, int mpr_cont)
        {
            this.emp_codi = emp_codi;
            this.mpr_cont = mpr_cont;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AplicarCDPResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class AplicarCDPResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string AplicarCDPResult;

        public AplicarCDPResponse()
        {
        }

        public AplicarCDPResponse(string AplicarCDPResult)
        {
            this.AplicarCDPResult = AplicarCDPResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AnularCDP", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class AnularCDPRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public int mpr_cont;

        public AnularCDPRequest()
        {
        }

        public AnularCDPRequest(int emp_codi, int mpr_cont)
        {
            this.emp_codi = emp_codi;
            this.mpr_cont = mpr_cont;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AnularCDPResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class AnularCDPResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string AnularCDPResult;

        public AnularCDPResponse()
        {
        }

        public AnularCDPResponse(string AnularCDPResult)
        {
            this.AnularCDPResult = AnularCDPResult;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://seven/")]
    public partial class TSPgMpres
    {

        private TSPgDmpre[] vDetalleField;

        private int emp_codiField;

        private int top_codiField;

        private int mpr_numeField;

        private System.DateTime mpr_fechField;

        private string mpr_descField;

        private string arb_csucField;

        private string ter_codaField;

        private string mpr_tdosField;

        private int mpr_ndosField;

        private int uni_codiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public TSPgDmpre[] vDetalle
        {
            get
            {
                return this.vDetalleField;
            }
            set
            {
                this.vDetalleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int Emp_codi
        {
            get
            {
                return this.emp_codiField;
            }
            set
            {
                this.emp_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int Top_codi
        {
            get
            {
                return this.top_codiField;
            }
            set
            {
                this.top_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int Mpr_nume
        {
            get
            {
                return this.mpr_numeField;
            }
            set
            {
                this.mpr_numeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public System.DateTime Mpr_fech
        {
            get
            {
                return this.mpr_fechField;
            }
            set
            {
                this.mpr_fechField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Mpr_desc
        {
            get
            {
                return this.mpr_descField;
            }
            set
            {
                this.mpr_descField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Arb_csuc
        {
            get
            {
                return this.arb_csucField;
            }
            set
            {
                this.arb_csucField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string Ter_coda
        {
            get
            {
                return this.ter_codaField;
            }
            set
            {
                this.ter_codaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string Mpr_tdos
        {
            get
            {
                return this.mpr_tdosField;
            }
            set
            {
                this.mpr_tdosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public int Mpr_ndos
        {
            get
            {
                return this.mpr_ndosField;
            }
            set
            {
                this.mpr_ndosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public int Uni_codi
        {
            get
            {
                return this.uni_codiField;
            }
            set
            {
                this.uni_codiField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://seven/")]
    public partial class TSPgDmpre
    {

        private string rub_codiField;

        private string pro_codiField;

        private string arb_codcField;

        private string arb_codaField;

        private string arb_codpField;

        private double dmp_valoField;

        private string dmp_descField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Rub_codi
        {
            get
            {
                return this.rub_codiField;
            }
            set
            {
                this.rub_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Pro_codi
        {
            get
            {
                return this.pro_codiField;
            }
            set
            {
                this.pro_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Arb_codc
        {
            get
            {
                return this.arb_codcField;
            }
            set
            {
                this.arb_codcField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Arb_coda
        {
            get
            {
                return this.arb_codaField;
            }
            set
            {
                this.arb_codaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Arb_codp
        {
            get
            {
                return this.arb_codpField;
            }
            set
            {
                this.arb_codpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public double Dmp_valo
        {
            get
            {
                return this.dmp_valoField;
            }
            set
            {
                this.dmp_valoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Dmp_desc
        {
            get
            {
                return this.dmp_descField;
            }
            set
            {
                this.dmp_descField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "CrearCDP", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class CrearCDPRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public TSPgMpres pCDP;

        public CrearCDPRequest()
        {
        }

        public CrearCDPRequest(TSPgMpres pCDP)
        {
            this.pCDP = pCDP;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "CrearCDPResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class CrearCDPResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string CrearCDPResult;

        public CrearCDPResponse()
        {
        }

        public CrearCDPResponse(string CrearCDPResult)
        {
            this.CrearCDPResult = CrearCDPResult;
        }
    }
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "CrearCuotaesponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class CrearCuotaResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string CrearCuotaResult;

        public CrearCuotaResponse()
        {
        }

        public CrearCuotaResponse(string CrearCuotaResult)
        {
            this.CrearCuotaResult = CrearCuotaResult;
        }
    }
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CrearCuotaMovContableResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CrearCuotaMovContableResponse", Namespace = "http://seven/", Order = 0)]
        public CrearCuotaMovContableResponseBody Body;

        public CrearCuotaMovContableResponse()
        {
        }

        public CrearCuotaMovContableResponse(CrearCuotaMovContableResponseBody Body)
        {
            this.Body = Body;
        }
    }
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class CrearCuotaMovContableResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string CrearCuotaResult;

        public CrearCuotaMovContableResponseBody()
        {
        }

        public CrearCuotaMovContableResponseBody(string CrearCuotaResult)
        {
            this.CrearCuotaResult = CrearCuotaResult;
        }
    }
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "CrearCuota", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class CrearCuotaRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public TSPgMpres pCuota;

        public CrearCuotaRequest()
        {
        }

        public CrearCuotaRequest(TSPgMpres pCuota)
        {
            this.pCuota = pCuota;
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface SPGOBLIGSoapChannel : SPGOBLIGSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class SPGOBLIGSoapClient : System.ServiceModel.ClientBase<SPGOBLIGSoap>, SPGOBLIGSoap
    {

        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public SPGOBLIGSoapClient(EndpointConfiguration endpointConfiguration) :
                base(SPGOBLIGSoapClient.GetBindingForEndpoint(endpointConfiguration), SPGOBLIGSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SPGOBLIGSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(SPGOBLIGSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SPGOBLIGSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(SPGOBLIGSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SPGOBLIGSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public InsertarCDPResponse InsertarCDP(InsertarCDPRequest request)
        {
            return base.Channel.InsertarCDP(request);
        }

        public System.Threading.Tasks.Task<InsertarCDPResponse> InsertarCDPAsync(InsertarCDPRequest request)
        {
            return base.Channel.InsertarCDPAsync(request);
        }

        public InsertarDetCDPResponse InsertarDetCDP(InsertarDetCDPRequest request)
        {
            return base.Channel.InsertarDetCDP(request);
        }

        public System.Threading.Tasks.Task<InsertarDetCDPResponse> InsertarDetCDPAsync(InsertarDetCDPRequest request)
        {
            return base.Channel.InsertarDetCDPAsync(request);
        }

        public AplicarCDPResponse AplicarCDP(AplicarCDPRequest request)
        {
            return base.Channel.AplicarCDP(request);
        }

        public System.Threading.Tasks.Task<AplicarCDPResponse> AplicarCDPAsync(AplicarCDPRequest request)
        {
            return base.Channel.AplicarCDPAsync(request);
        }

        public AnularCDPResponse AnularCDP(AnularCDPRequest request)
        {
            return base.Channel.AnularCDP(request);
        }

        public System.Threading.Tasks.Task<AnularCDPResponse> AnularCDPAsync(AnularCDPRequest request)
        {
            return base.Channel.AnularCDPAsync(request);
        }

        public CrearCDPResponse CrearCDP(CrearCDPRequest request)
        {
            return base.Channel.CrearCDP(request);
        }

        public System.Threading.Tasks.Task<CrearCDPResponse> CrearCDPAsync(CrearCDPRequest request)
        {
            return base.Channel.CrearCDPAsync(request);
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
            if ((endpointConfiguration == EndpointConfiguration.SPGOBLIGSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.SPGOBLIGSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.SPGOBLIGSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSCdp/WPgOblig.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.SPGOBLIGSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSCdp/WPgOblig.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            SPGOBLIGSoap,

            SPGOBLIGSoap12,
        }
    }
}
