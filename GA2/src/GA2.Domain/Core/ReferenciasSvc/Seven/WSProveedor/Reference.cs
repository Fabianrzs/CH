namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://seven/", ConfigurationName = "SIEWSSECSoap")]
    public interface SIEWSSECSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncGnTerce", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        SyncGnTerceResponse SyncGnTerce(SyncGnTerceRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncGnTerce", ReplyAction = "*")]
        System.Threading.Tasks.Task<SyncGnTerceResponse> SyncGnTerceAsync(SyncGnTerceRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncFaClien", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        SyncFaClienResponse SyncFaClien(SyncFaClienRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncFaClien", ReplyAction = "*")]
        System.Threading.Tasks.Task<SyncFaClienResponse> SyncFaClienAsync(SyncFaClienRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncFaClienComplex", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        SyncFaClienComplexResponse SyncFaClienComplex(SyncFaClienComplexRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncFaClienComplex", ReplyAction = "*")]
        System.Threading.Tasks.Task<SyncFaClienComplexResponse> SyncFaClienComplexAsync(SyncFaClienComplexRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncPoPvdor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        SyncPoPvdorResponse SyncPoPvdor(SyncPoPvdorRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncPoPvdor", ReplyAction = "*")]
        System.Threading.Tasks.Task<SyncPoPvdorResponse> SyncPoPvdorAsync(SyncPoPvdorRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncFaConta", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        SyncFaContaResponse SyncFaConta(SyncFaContaRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncFaConta", ReplyAction = "*")]
        System.Threading.Tasks.Task<SyncFaContaResponse> SyncFaContaAsync(SyncFaContaRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncEmpleadoPvdor", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        SyncEmpleadoPvdorResponse SyncEmpleadoPvdor(SyncEmpleadoPvdorRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SyncEmpleadoPvdor", ReplyAction = "*")]
        System.Threading.Tasks.Task<SyncEmpleadoPvdorResponse> SyncEmpleadoPvdorAsync(SyncEmpleadoPvdorRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/ChangeNit", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        ChangeNitResponse ChangeNit(ChangeNitRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/ChangeNit", ReplyAction = "*")]
        System.Threading.Tasks.Task<ChangeNitResponse> ChangeNitAsync(ChangeNitRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/ActModalidadCupo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        ActModalidadCupoResponse ActModalidadCupo(ActModalidadCupoRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/ActModalidadCupo", ReplyAction = "*")]
        System.Threading.Tasks.Task<ActModalidadCupoResponse> ActModalidadCupoAsync(ActModalidadCupoRequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncGnTerce", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncGnTerceRequest
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
        public string ter_noco;

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
        public string ter_audp;

        public SyncGnTerceRequest()
        {
        }

        public SyncGnTerceRequest(
                    int emp_codi,
                    int tip_codi,
                    string ter_coda,
                    int ter_dive,
                    string ter_nomb,
                    string ter_apel,
                    string ter_noco,
                    int mod_codi,
                    int pai_codi,
                    int dep_codi,
                    int mun_codi,
                    string ter_dire,
                    string ter_ntel,
                    string ter_mail,
                    string ter_nfax,
                    string ter_audp)
        {
            this.emp_codi = emp_codi;
            this.tip_codi = tip_codi;
            this.ter_coda = ter_coda;
            this.ter_dive = ter_dive;
            this.ter_nomb = ter_nomb;
            this.ter_apel = ter_apel;
            this.ter_noco = ter_noco;
            this.mod_codi = mod_codi;
            this.pai_codi = pai_codi;
            this.dep_codi = dep_codi;
            this.mun_codi = mun_codi;
            this.ter_dire = ter_dire;
            this.ter_ntel = ter_ntel;
            this.ter_mail = ter_mail;
            this.ter_nfax = ter_nfax;
            this.ter_audp = ter_audp;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncGnTerceResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncGnTerceResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string SyncGnTerceResult;

        public SyncGnTerceResponse()
        {
        }

        public SyncGnTerceResponse(string SyncGnTerceResult)
        {
            this.SyncGnTerceResult = SyncGnTerceResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncFaClien", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncFaClienRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public int tip_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public string ter_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public int cli_dive;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 4)]
        public string cli_nomb;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 5)]
        public string cli_apel;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 6)]
        public string cli_noco;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 7)]
        public int mod_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 8)]
        public int pai_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 9)]
        public int dep_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 10)]
        public int mun_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 11)]
        public string cli_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 12)]
        public int tcl_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 13)]
        public int cal_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 14)]
        public int coc_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 15)]
        public int cim_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 16)]
        public string arb_csuc;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 17)]
        public string dcl_dire;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 18)]
        public string dcl_ntel;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 19)]
        public string dcl_mail;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 20)]
        public string dcl_nfax;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 21)]
        public string arb_clte;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 22)]
        public string cli_inna;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 23)]
        public double ven_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 24)]
        public string arb_ccec;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 25)]
        public int lis_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 26)]
        public string dcl_obse;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 27)]
        public string dcl_apar;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 28)]
        public string dcl_cloc;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 29)]
        public string cli_anex;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 30)]
        public string cli_hpra;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 31)]
        public int cli_fecm;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 32)]
        public int cli_feca;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 33)]
        public string cli_audp;

        public SyncFaClienRequest()
        {
        }

        public SyncFaClienRequest(
                    int emp_codi,
                    int tip_codi,
                    string ter_coda,
                    int cli_dive,
                    string cli_nomb,
                    string cli_apel,
                    string cli_noco,
                    int mod_codi,
                    int pai_codi,
                    int dep_codi,
                    int mun_codi,
                    string cli_coda,
                    int tcl_codi,
                    int cal_codi,
                    int coc_codi,
                    int cim_codi,
                    string arb_csuc,
                    string dcl_dire,
                    string dcl_ntel,
                    string dcl_mail,
                    string dcl_nfax,
                    string arb_clte,
                    string cli_inna,
                    double ven_codi,
                    string arb_ccec,
                    int lis_codi,
                    string dcl_obse,
                    string dcl_apar,
                    string dcl_cloc,
                    string cli_anex,
                    string cli_hpra,
                    int cli_fecm,
                    int cli_feca,
                    string cli_audp)
        {
            this.emp_codi = emp_codi;
            this.tip_codi = tip_codi;
            this.ter_coda = ter_coda;
            this.cli_dive = cli_dive;
            this.cli_nomb = cli_nomb;
            this.cli_apel = cli_apel;
            this.cli_noco = cli_noco;
            this.mod_codi = mod_codi;
            this.pai_codi = pai_codi;
            this.dep_codi = dep_codi;
            this.mun_codi = mun_codi;
            this.cli_coda = cli_coda;
            this.tcl_codi = tcl_codi;
            this.cal_codi = cal_codi;
            this.coc_codi = coc_codi;
            this.cim_codi = cim_codi;
            this.arb_csuc = arb_csuc;
            this.dcl_dire = dcl_dire;
            this.dcl_ntel = dcl_ntel;
            this.dcl_mail = dcl_mail;
            this.dcl_nfax = dcl_nfax;
            this.arb_clte = arb_clte;
            this.cli_inna = cli_inna;
            this.ven_codi = ven_codi;
            this.arb_ccec = arb_ccec;
            this.lis_codi = lis_codi;
            this.dcl_obse = dcl_obse;
            this.dcl_apar = dcl_apar;
            this.dcl_cloc = dcl_cloc;
            this.cli_anex = cli_anex;
            this.cli_hpra = cli_hpra;
            this.cli_fecm = cli_fecm;
            this.cli_feca = cli_feca;
            this.cli_audp = cli_audp;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncFaClienResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncFaClienResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string SyncFaClienResult;

        public SyncFaClienResponse()
        {
        }

        public SyncFaClienResponse(string SyncFaClienResult)
        {
            this.SyncFaClienResult = SyncFaClienResult;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://seven/")]
    public partial class TOFaClien
    {

        private int emp_codiField;

        private int tip_codiField;

        private string ter_codaField;

        private int cli_diveField;

        private string cli_nombField;

        private string cli_apelField;

        private string cli_nocoField;

        private int mod_codiField;

        private int pai_codiField;

        private int dep_codiField;

        private int mun_codiField;

        private string cli_codaField;

        private int tcl_codiField;

        private int cal_codiField;

        private int coc_codiField;

        private int cim_codiField;

        private string arb_csucField;

        private string dcl_direField;

        private string dcl_ntelField;

        private string dcl_mailField;

        private string dcl_nfaxField;

        private string arb_clteField;

        private string cli_innaField;

        private double ven_codiField;

        private string arb_ccecField;

        private int lis_codiField;

        private string dcl_obseField;

        private string dcl_aparField;

        private string dcl_clocField;

        private string cli_anexField;

        private string cli_hpraField;

        private int cli_fecmField;

        private int cli_fecaField;

        private string cli_audpField;

        private string arb_cproField;

        private string arb_careField;

        private string act_codiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int emp_codi
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int tip_codi
        {
            get
            {
                return this.tip_codiField;
            }
            set
            {
                this.tip_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ter_coda
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int cli_dive
        {
            get
            {
                return this.cli_diveField;
            }
            set
            {
                this.cli_diveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string cli_nomb
        {
            get
            {
                return this.cli_nombField;
            }
            set
            {
                this.cli_nombField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string cli_apel
        {
            get
            {
                return this.cli_apelField;
            }
            set
            {
                this.cli_apelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string cli_noco
        {
            get
            {
                return this.cli_nocoField;
            }
            set
            {
                this.cli_nocoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public int mod_codi
        {
            get
            {
                return this.mod_codiField;
            }
            set
            {
                this.mod_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public int pai_codi
        {
            get
            {
                return this.pai_codiField;
            }
            set
            {
                this.pai_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public int dep_codi
        {
            get
            {
                return this.dep_codiField;
            }
            set
            {
                this.dep_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public int mun_codi
        {
            get
            {
                return this.mun_codiField;
            }
            set
            {
                this.mun_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string cli_coda
        {
            get
            {
                return this.cli_codaField;
            }
            set
            {
                this.cli_codaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public int tcl_codi
        {
            get
            {
                return this.tcl_codiField;
            }
            set
            {
                this.tcl_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public int cal_codi
        {
            get
            {
                return this.cal_codiField;
            }
            set
            {
                this.cal_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public int coc_codi
        {
            get
            {
                return this.coc_codiField;
            }
            set
            {
                this.coc_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public int cim_codi
        {
            get
            {
                return this.cim_codiField;
            }
            set
            {
                this.cim_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string arb_csuc
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string dcl_dire
        {
            get
            {
                return this.dcl_direField;
            }
            set
            {
                this.dcl_direField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string dcl_ntel
        {
            get
            {
                return this.dcl_ntelField;
            }
            set
            {
                this.dcl_ntelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string dcl_mail
        {
            get
            {
                return this.dcl_mailField;
            }
            set
            {
                this.dcl_mailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string dcl_nfax
        {
            get
            {
                return this.dcl_nfaxField;
            }
            set
            {
                this.dcl_nfaxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string arb_clte
        {
            get
            {
                return this.arb_clteField;
            }
            set
            {
                this.arb_clteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string cli_inna
        {
            get
            {
                return this.cli_innaField;
            }
            set
            {
                this.cli_innaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public double ven_codi
        {
            get
            {
                return this.ven_codiField;
            }
            set
            {
                this.ven_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string arb_ccec
        {
            get
            {
                return this.arb_ccecField;
            }
            set
            {
                this.arb_ccecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public int lis_codi
        {
            get
            {
                return this.lis_codiField;
            }
            set
            {
                this.lis_codiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public string dcl_obse
        {
            get
            {
                return this.dcl_obseField;
            }
            set
            {
                this.dcl_obseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public string dcl_apar
        {
            get
            {
                return this.dcl_aparField;
            }
            set
            {
                this.dcl_aparField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
        public string dcl_cloc
        {
            get
            {
                return this.dcl_clocField;
            }
            set
            {
                this.dcl_clocField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
        public string cli_anex
        {
            get
            {
                return this.cli_anexField;
            }
            set
            {
                this.cli_anexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 30)]
        public string cli_hpra
        {
            get
            {
                return this.cli_hpraField;
            }
            set
            {
                this.cli_hpraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 31)]
        public int cli_fecm
        {
            get
            {
                return this.cli_fecmField;
            }
            set
            {
                this.cli_fecmField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 32)]
        public int cli_feca
        {
            get
            {
                return this.cli_fecaField;
            }
            set
            {
                this.cli_fecaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 33)]
        public string cli_audp
        {
            get
            {
                return this.cli_audpField;
            }
            set
            {
                this.cli_audpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 34)]
        public string arb_cpro
        {
            get
            {
                return this.arb_cproField;
            }
            set
            {
                this.arb_cproField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 35)]
        public string arb_care
        {
            get
            {
                return this.arb_careField;
            }
            set
            {
                this.arb_careField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 36)]
        public string act_codi
        {
            get
            {
                return this.act_codiField;
            }
            set
            {
                this.act_codiField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://seven/")]
    public partial class TSalida
    {

        private short retornoField;

        private string txtErrorField;

        private int cliCodiField;

        private short dclCoddField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public short Retorno
        {
            get
            {
                return this.retornoField;
            }
            set
            {
                this.retornoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string TxtError
        {
            get
            {
                return this.txtErrorField;
            }
            set
            {
                this.txtErrorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int CliCodi
        {
            get
            {
                return this.cliCodiField;
            }
            set
            {
                this.cliCodiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public short DclCodd
        {
            get
            {
                return this.dclCoddField;
            }
            set
            {
                this.dclCoddField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncFaClienComplex", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncFaClienComplexRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public TOFaClien toFaClient;

        public SyncFaClienComplexRequest()
        {
        }

        public SyncFaClienComplexRequest(TOFaClien toFaClient)
        {
            this.toFaClient = toFaClient;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncFaClienComplexResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncFaClienComplexResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public TSalida SyncFaClienComplexResult;

        public SyncFaClienComplexResponse()
        {
        }

        public SyncFaClienComplexResponse(TSalida SyncFaClienComplexResult)
        {
            this.SyncFaClienComplexResult = SyncFaClienComplexResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncPoPvdor", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncPoPvdorRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public int tip_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public string pvd_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public int pvd_dive;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 4)]
        public string pvd_nomb;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 5)]
        public string pvd_apel;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 6)]
        public string pvd_noco;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 7)]
        public int pai_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 8)]
        public int dep_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 9)]
        public int mun_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 10)]
        public int tpr_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 11)]
        public string pvd_clad;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 12)]
        public string arb_csuc;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 13)]
        public int cal_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 14)]
        public int coc_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 15)]
        public int cim_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 16)]
        public int act_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 17)]
        public int mon_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 18)]
        public string cup_nume;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 19)]
        public string cup_idba;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 20)]
        public int ban_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 21)]
        public int sub_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 22)]
        public string cup_tipo;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 23)]
        public string pvr_riva;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 24)]
        public string pvr_auto;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 25)]
        public string pvr_reau;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 26)]
        public string cup_tdba;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 27)]
        public string cup_tcta;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 28)]
        public string cup_ib04;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 29)]
        public string cup_ib05;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 30)]
        public string cup_ib06;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 31)]
        public string cup_ib07;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 32)]
        public string cup_ib08;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 33)]
        public string cup_ib09;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 34)]
        public string cup_ib10;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 35)]
        public string dep_dire;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 36)]
        public string dep_ntel;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 37)]
        public string dep_mail;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 38)]
        public int dep_codd;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 39)]
        public string cup_numi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 40)]
        public string cup_nomi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 41)]
        public int ban_coin;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 42)]
        public string ban_dire;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 43)]
        public string cup_coab;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 44)]
        public string cup_coib;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 45)]
        public string cup_coas;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 46)]
        public string cup_cobi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 47)]
        public string cup_clas;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 48)]
        public string dep_apar;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 49)]
        public string dep_nfax;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 50)]
        public string dep_cloc;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 51)]
        public string dep_nomb;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 52)]
        public string cup_esta;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 53)]
        public string pvd_audp;

        public SyncPoPvdorRequest()
        {
        }

        public SyncPoPvdorRequest(
                    int emp_codi,
                    int tip_codi,
                    string pvd_coda,
                    int pvd_dive,
                    string pvd_nomb,
                    string pvd_apel,
                    string pvd_noco,
                    int pai_codi,
                    int dep_codi,
                    int mun_codi,
                    int tpr_codi,
                    string pvd_clad,
                    string arb_csuc,
                    int cal_codi,
                    int coc_codi,
                    int cim_codi,
                    int act_codi,
                    int mon_codi,
                    string cup_nume,
                    string cup_idba,
                    int ban_codi,
                    int sub_codi,
                    string cup_tipo,
                    string pvr_riva,
                    string pvr_auto,
                    string pvr_reau,
                    string cup_tdba,
                    string cup_tcta,
                    string cup_ib04,
                    string cup_ib05,
                    string cup_ib06,
                    string cup_ib07,
                    string cup_ib08,
                    string cup_ib09,
                    string cup_ib10,
                    string dep_dire,
                    string dep_ntel,
                    string dep_mail,
                    int dep_codd,
                    string cup_numi,
                    string cup_nomi,
                    int ban_coin,
                    string ban_dire,
                    string cup_coab,
                    string cup_coib,
                    string cup_coas,
                    string cup_cobi,
                    string cup_clas,
                    string dep_apar,
                    string dep_nfax,
                    string dep_cloc,
                    string dep_nomb,
                    string cup_esta,
                    string pvd_audp)
        {
            this.emp_codi = emp_codi;
            this.tip_codi = tip_codi;
            this.pvd_coda = pvd_coda;
            this.pvd_dive = pvd_dive;
            this.pvd_nomb = pvd_nomb;
            this.pvd_apel = pvd_apel;
            this.pvd_noco = pvd_noco;
            this.pai_codi = pai_codi;
            this.dep_codi = dep_codi;
            this.mun_codi = mun_codi;
            this.tpr_codi = tpr_codi;
            this.pvd_clad = pvd_clad;
            this.arb_csuc = arb_csuc;
            this.cal_codi = cal_codi;
            this.coc_codi = coc_codi;
            this.cim_codi = cim_codi;
            this.act_codi = act_codi;
            this.mon_codi = mon_codi;
            this.cup_nume = cup_nume;
            this.cup_idba = cup_idba;
            this.ban_codi = ban_codi;
            this.sub_codi = sub_codi;
            this.cup_tipo = cup_tipo;
            this.pvr_riva = pvr_riva;
            this.pvr_auto = pvr_auto;
            this.pvr_reau = pvr_reau;
            this.cup_tdba = cup_tdba;
            this.cup_tcta = cup_tcta;
            this.cup_ib04 = cup_ib04;
            this.cup_ib05 = cup_ib05;
            this.cup_ib06 = cup_ib06;
            this.cup_ib07 = cup_ib07;
            this.cup_ib08 = cup_ib08;
            this.cup_ib09 = cup_ib09;
            this.cup_ib10 = cup_ib10;
            this.dep_dire = dep_dire;
            this.dep_ntel = dep_ntel;
            this.dep_mail = dep_mail;
            this.dep_codd = dep_codd;
            this.cup_numi = cup_numi;
            this.cup_nomi = cup_nomi;
            this.ban_coin = ban_coin;
            this.ban_dire = ban_dire;
            this.cup_coab = cup_coab;
            this.cup_coib = cup_coib;
            this.cup_coas = cup_coas;
            this.cup_cobi = cup_cobi;
            this.cup_clas = cup_clas;
            this.dep_apar = dep_apar;
            this.dep_nfax = dep_nfax;
            this.dep_cloc = dep_cloc;
            this.dep_nomb = dep_nomb;
            this.cup_esta = cup_esta;
            this.pvd_audp = pvd_audp;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncPoPvdorResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncPoPvdorResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string SyncPoPvdorResult;

        public SyncPoPvdorResponse()
        {
        }

        public SyncPoPvdorResponse(string SyncPoPvdorResult)
        {
            this.SyncPoPvdorResult = SyncPoPvdorResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncFaConta", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncFaContaRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public string cli_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public int dcl_codd;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public int con_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 4)]
        public string con_nomb;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 5)]
        public string con_carg;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 6)]
        public string con_dire;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 7)]
        public string con_dibo;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 8)]
        public string con_tele;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 9)]
        public string con_nufa;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 10)]
        public string con_mail;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 11)]
        public System.DateTime con_fcum;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 12)]
        public string con_afic;

        public SyncFaContaRequest()
        {
        }

        public SyncFaContaRequest(int emp_codi, string cli_coda, int dcl_codd, int con_codi, string con_nomb, string con_carg, string con_dire, string con_dibo, string con_tele, string con_nufa, string con_mail, System.DateTime con_fcum, string con_afic)
        {
            this.emp_codi = emp_codi;
            this.cli_coda = cli_coda;
            this.dcl_codd = dcl_codd;
            this.con_codi = con_codi;
            this.con_nomb = con_nomb;
            this.con_carg = con_carg;
            this.con_dire = con_dire;
            this.con_dibo = con_dibo;
            this.con_tele = con_tele;
            this.con_nufa = con_nufa;
            this.con_mail = con_mail;
            this.con_fcum = con_fcum;
            this.con_afic = con_afic;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncFaContaResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncFaContaResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string SyncFaContaResult;

        public SyncFaContaResponse()
        {
        }

        public SyncFaContaResponse(string SyncFaContaResult)
        {
            this.SyncFaContaResult = SyncFaContaResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncEmpleadoPvdor", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncEmpleadoPvdorRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public int tip_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public string pvd_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public int pvd_dive;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 4)]
        public string pvd_nomb;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 5)]
        public string pvd_apel;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 6)]
        public string pvd_noco;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 7)]
        public int pai_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 8)]
        public int dep_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 9)]
        public int mun_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 10)]
        public string arb_csuc;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 11)]
        public string cup_nume;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 12)]
        public int ban_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 13)]
        public int sub_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 14)]
        public string cup_tipo;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 15)]
        public string dep_dire;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 16)]
        public string dep_ntel;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 17)]
        public string dep_mail;

        public SyncEmpleadoPvdorRequest()
        {
        }

        public SyncEmpleadoPvdorRequest(
                    int emp_codi,
                    int tip_codi,
                    string pvd_coda,
                    int pvd_dive,
                    string pvd_nomb,
                    string pvd_apel,
                    string pvd_noco,
                    int pai_codi,
                    int dep_codi,
                    int mun_codi,
                    string arb_csuc,
                    string cup_nume,
                    int ban_codi,
                    int sub_codi,
                    string cup_tipo,
                    string dep_dire,
                    string dep_ntel,
                    string dep_mail)
        {
            this.emp_codi = emp_codi;
            this.tip_codi = tip_codi;
            this.pvd_coda = pvd_coda;
            this.pvd_dive = pvd_dive;
            this.pvd_nomb = pvd_nomb;
            this.pvd_apel = pvd_apel;
            this.pvd_noco = pvd_noco;
            this.pai_codi = pai_codi;
            this.dep_codi = dep_codi;
            this.mun_codi = mun_codi;
            this.arb_csuc = arb_csuc;
            this.cup_nume = cup_nume;
            this.ban_codi = ban_codi;
            this.sub_codi = sub_codi;
            this.cup_tipo = cup_tipo;
            this.dep_dire = dep_dire;
            this.dep_ntel = dep_ntel;
            this.dep_mail = dep_mail;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SyncEmpleadoPvdorResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class SyncEmpleadoPvdorResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string SyncEmpleadoPvdorResult;

        public SyncEmpleadoPvdorResponse()
        {
        }

        public SyncEmpleadoPvdorResponse(string SyncEmpleadoPvdorResult)
        {
            this.SyncEmpleadoPvdorResult = SyncEmpleadoPvdorResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ChangeNit", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class ChangeNitRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public string ter_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public string ter_codn;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public int tip_codn;

        public ChangeNitRequest()
        {
        }

        public ChangeNitRequest(int emp_codi, string ter_coda, string ter_codn, int tip_codn)
        {
            this.emp_codi = emp_codi;
            this.ter_coda = ter_coda;
            this.ter_codn = ter_codn;
            this.tip_codn = tip_codn;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ChangeNitResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class ChangeNitResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string ChangeNitResult;

        public ChangeNitResponse()
        {
        }

        public ChangeNitResponse(string ChangeNitResult)
        {
            this.ChangeNitResult = ChangeNitResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ActModalidadCupo", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class ActModalidadCupoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public int emp_codi;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 1)]
        public string cli_coda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 2)]
        public string arb_codc;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 3)]
        public string ite_moda;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 4)]
        public double dcl_cupo;

        public ActModalidadCupoRequest()
        {
        }

        public ActModalidadCupoRequest(int emp_codi, string cli_coda, string arb_codc, string ite_moda, double dcl_cupo)
        {
            this.emp_codi = emp_codi;
            this.cli_coda = cli_coda;
            this.arb_codc = arb_codc;
            this.ite_moda = ite_moda;
            this.dcl_cupo = dcl_cupo;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ActModalidadCupoResponse", WrapperNamespace = "http://seven/", IsWrapped = true)]
    public partial class ActModalidadCupoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://seven/", Order = 0)]
        public string ActModalidadCupoResult;

        public ActModalidadCupoResponse()
        {
        }

        public ActModalidadCupoResponse(string ActModalidadCupoResult)
        {
            this.ActModalidadCupoResult = ActModalidadCupoResult;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface SIEWSSECSoapChannel : SIEWSSECSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class SIEWSSECSoapClient : System.ServiceModel.ClientBase<SIEWSSECSoap>, SIEWSSECSoap
    {

        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public SIEWSSECSoapClient(EndpointConfiguration endpointConfiguration) :
                base(SIEWSSECSoapClient.GetBindingForEndpoint(endpointConfiguration), SIEWSSECSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SIEWSSECSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(SIEWSSECSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SIEWSSECSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(SIEWSSECSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SIEWSSECSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public SyncGnTerceResponse SyncGnTerce(SyncGnTerceRequest request)
        {
            return base.Channel.SyncGnTerce(request);
        }

        public System.Threading.Tasks.Task<SyncGnTerceResponse> SyncGnTerceAsync(SyncGnTerceRequest request)
        {
            return base.Channel.SyncGnTerceAsync(request);
        }

        public SyncFaClienResponse SyncFaClien(SyncFaClienRequest request)
        {
            return base.Channel.SyncFaClien(request);
        }

        public System.Threading.Tasks.Task<SyncFaClienResponse> SyncFaClienAsync(SyncFaClienRequest request)
        {
            return base.Channel.SyncFaClienAsync(request);
        }

        public SyncFaClienComplexResponse SyncFaClienComplex(SyncFaClienComplexRequest request)
        {
            return base.Channel.SyncFaClienComplex(request);
        }

        public System.Threading.Tasks.Task<SyncFaClienComplexResponse> SyncFaClienComplexAsync(SyncFaClienComplexRequest request)
        {
            return base.Channel.SyncFaClienComplexAsync(request);
        }

        public SyncPoPvdorResponse SyncPoPvdor(SyncPoPvdorRequest request)
        {
            return base.Channel.SyncPoPvdor(request);
        }

        public System.Threading.Tasks.Task<SyncPoPvdorResponse> SyncPoPvdorAsync(SyncPoPvdorRequest request)
        {
            return base.Channel.SyncPoPvdorAsync(request);
        }

        public SyncFaContaResponse SyncFaConta(SyncFaContaRequest request)
        {
            return base.Channel.SyncFaConta(request);
        }

        public System.Threading.Tasks.Task<SyncFaContaResponse> SyncFaContaAsync(SyncFaContaRequest request)
        {
            return base.Channel.SyncFaContaAsync(request);
        }

        public SyncEmpleadoPvdorResponse SyncEmpleadoPvdor(SyncEmpleadoPvdorRequest request)
        {
            return base.Channel.SyncEmpleadoPvdor(request);
        }

        public System.Threading.Tasks.Task<SyncEmpleadoPvdorResponse> SyncEmpleadoPvdorAsync(SyncEmpleadoPvdorRequest request)
        {
            return base.Channel.SyncEmpleadoPvdorAsync(request);
        }

        public ChangeNitResponse ChangeNit(ChangeNitRequest request)
        {
            return base.Channel.ChangeNit(request);
        }

        public System.Threading.Tasks.Task<ChangeNitResponse> ChangeNitAsync(ChangeNitRequest request)
        {
            return base.Channel.ChangeNitAsync(request);
        }

        public ActModalidadCupoResponse ActModalidadCupo(ActModalidadCupoRequest request)
        {
            return base.Channel.ActModalidadCupo(request);
        }

        public System.Threading.Tasks.Task<ActModalidadCupoResponse> ActModalidadCupoAsync(ActModalidadCupoRequest request)
        {
            return base.Channel.ActModalidadCupoAsync(request);
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
            if ((endpointConfiguration == EndpointConfiguration.SIEWSSECSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.SIEWSSECSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.SIEWSSECSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSProveedor/SIeWssec.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.SIEWSSECSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSProveedor/SIeWssec.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            SIEWSSECSoap,

            SIEWSSECSoap12,
        }
    }
}
