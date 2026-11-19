namespace GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "TSCnMcont", Namespace = "http://seven/")]
    public partial class TSCnMcont : object
    {

        private TSCnDmcon[] vDetalleField;

        private int Emp_codiField;

        private int Top_codiField;

        private int Mco_numeField;

        private System.DateTime Mco_fechField;

        private int Mod_codiField;

        private string Arb_csucField;

        private string Mco_descField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public TSCnDmcon[] vDetalle
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public int Emp_codi
        {
            get
            {
                return this.Emp_codiField;
            }
            set
            {
                this.Emp_codiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public int Top_codi
        {
            get
            {
                return this.Top_codiField;
            }
            set
            {
                this.Top_codiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int Mco_nume
        {
            get
            {
                return this.Mco_numeField;
            }
            set
            {
                this.Mco_numeField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public System.DateTime Mco_fech
        {
            get
            {
                return this.Mco_fechField;
            }
            set
            {
                this.Mco_fechField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int Mod_codi
        {
            get
            {
                return this.Mod_codiField;
            }
            set
            {
                this.Mod_codiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string Arb_csuc
        {
            get
            {
                return this.Arb_csucField;
            }
            set
            {
                this.Arb_csucField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string Mco_desc
        {
            get
            {
                return this.Mco_descField;
            }
            set
            {
                this.Mco_descField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "TSCnDmcon", Namespace = "http://seven/")]
    public partial class TSCnDmcon : object
    {

        private string Cue_codiField;

        private string Dmc_descField;

        private double Dmc_cantField;

        private double Dmc_actiField;

        private string Dmc_refeField;

        private decimal Dmc_vadbField;

        private decimal Dmc_vacrField;

        private decimal Dmc_vabaField;

        private string Ter_codaField;

        private string Arb_codcField;

        private string Arb_codaField;

        private string Arb_codpField;

        private string Arb_codsField;

        private string Ter_codmField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string Cue_codi
        {
            get
            {
                return this.Cue_codiField;
            }
            set
            {
                this.Cue_codiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string Dmc_desc
        {
            get
            {
                return this.Dmc_descField;
            }
            set
            {
                this.Dmc_descField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public double Dmc_cant
        {
            get
            {
                return this.Dmc_cantField;
            }
            set
            {
                this.Dmc_cantField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public double Dmc_acti
        {
            get
            {
                return this.Dmc_actiField;
            }
            set
            {
                this.Dmc_actiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string Dmc_refe
        {
            get
            {
                return this.Dmc_refeField;
            }
            set
            {
                this.Dmc_refeField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public decimal Dmc_vadb
        {
            get
            {
                return this.Dmc_vadbField;
            }
            set
            {
                this.Dmc_vadbField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public decimal Dmc_vacr
        {
            get
            {
                return this.Dmc_vacrField;
            }
            set
            {
                this.Dmc_vacrField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public decimal Dmc_vaba
        {
            get
            {
                return this.Dmc_vabaField;
            }
            set
            {
                this.Dmc_vabaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string Ter_coda
        {
            get
            {
                return this.Ter_codaField;
            }
            set
            {
                this.Ter_codaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string Arb_codc
        {
            get
            {
                return this.Arb_codcField;
            }
            set
            {
                this.Arb_codcField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string Arb_coda
        {
            get
            {
                return this.Arb_codaField;
            }
            set
            {
                this.Arb_codaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string Arb_codp
        {
            get
            {
                return this.Arb_codpField;
            }
            set
            {
                this.Arb_codpField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string Arb_cods
        {
            get
            {
                return this.Arb_codsField;
            }
            set
            {
                this.Arb_codsField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string Ter_codm
        {
            get
            {
                return this.Ter_codmField;
            }
            set
            {
                this.Ter_codmField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "TToper", Namespace = "http://seven/")]
    public partial class TToper : object
    {

        private int Mco_secuField;

        private int Emp_codiField;

        private int Top_codiField;

        private int Mco_numeField;

        private double Mco_nechField;

        private string Arb_codsField;

        private string Ter_codaField;

        private string Mco_descField;

        private decimal Mco_valoField;

        private string Arb_codcField;

        private string Arb_codpField;

        private string Arb_codaField;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int Mco_secu
        {
            get
            {
                return this.Mco_secuField;
            }
            set
            {
                this.Mco_secuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public int Emp_codi
        {
            get
            {
                return this.Emp_codiField;
            }
            set
            {
                this.Emp_codiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public int Top_codi
        {
            get
            {
                return this.Top_codiField;
            }
            set
            {
                this.Top_codiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int Mco_nume
        {
            get
            {
                return this.Mco_numeField;
            }
            set
            {
                this.Mco_numeField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public double Mco_nech
        {
            get
            {
                return this.Mco_nechField;
            }
            set
            {
                this.Mco_nechField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string Arb_cods
        {
            get
            {
                return this.Arb_codsField;
            }
            set
            {
                this.Arb_codsField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string Ter_coda
        {
            get
            {
                return this.Ter_codaField;
            }
            set
            {
                this.Ter_codaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string Mco_desc
        {
            get
            {
                return this.Mco_descField;
            }
            set
            {
                this.Mco_descField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public decimal Mco_valo
        {
            get
            {
                return this.Mco_valoField;
            }
            set
            {
                this.Mco_valoField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string Arb_codc
        {
            get
            {
                return this.Arb_codcField;
            }
            set
            {
                this.Arb_codcField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string Arb_codp
        {
            get
            {
                return this.Arb_codpField;
            }
            set
            {
                this.Arb_codpField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string Arb_coda
        {
            get
            {
                return this.Arb_codaField;
            }
            set
            {
                this.Arb_codaField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "TRetToper", Namespace = "http://seven/")]
    public partial class TRetToper : object
    {

        private int Mco_secuField;

        private string RetornoField;

        private string TxtErrorField;

        private int Mco_contField;

        private int Mco_numeField;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int Mco_secu
        {
            get
            {
                return this.Mco_secuField;
            }
            set
            {
                this.Mco_secuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string Retorno
        {
            get
            {
                return this.RetornoField;
            }
            set
            {
                this.RetornoField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TxtError
        {
            get
            {
                return this.TxtErrorField;
            }
            set
            {
                this.TxtErrorField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int Mco_cont
        {
            get
            {
                return this.Mco_contField;
            }
            set
            {
                this.Mco_contField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int Mco_nume
        {
            get
            {
                return this.Mco_numeField;
            }
            set
            {
                this.Mco_numeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://seven/", ConfigurationName = "SCNMCONTSoap")]
    public interface SCNMCONTSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarCnMcont", ReplyAction = "*")]
        InsertarCnMcontResponse InsertarCnMcont(InsertarCnMcontRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarCnMcont", ReplyAction = "*")]
        System.Threading.Tasks.Task<InsertarCnMcontResponse> InsertarCnMcontAsync(InsertarCnMcontRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarCnDmcon", ReplyAction = "*")]
        InsertarCnDmconResponse InsertarCnDmcon(InsertarCnDmconRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarCnDmcon", ReplyAction = "*")]
        System.Threading.Tasks.Task<InsertarCnDmconResponse> InsertarCnDmconAsync(InsertarCnDmconRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/AplicarCnMcont", ReplyAction = "*")]
        AplicarCnMcontResponse AplicarCnMcont(AplicarCnMcontRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/AplicarCnMcont", ReplyAction = "*")]
        System.Threading.Tasks.Task<AplicarCnMcontResponse> AplicarCnMcontAsync(AplicarCnMcontRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/RevertirCnMcont", ReplyAction = "*")]
        RevertirCnMcontResponse RevertirCnMcont(RevertirCnMcontRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/RevertirCnMcont", ReplyAction = "*")]
        System.Threading.Tasks.Task<RevertirCnMcontResponse> RevertirCnMcontAsync(RevertirCnMcontRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/AnularCnMcont", ReplyAction = "*")]
        AnularCnMcontResponse AnularCnMcont(AnularCnMcontRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/AnularCnMcont", ReplyAction = "*")]
        System.Threading.Tasks.Task<AnularCnMcontResponse> AnularCnMcontAsync(AnularCnMcontRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarMovContable", ReplyAction = "*")]
        InsertarMovContableResponse InsertarMovContable(InsertarMovContableRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/InsertarMovContable", ReplyAction = "*")]
        System.Threading.Tasks.Task<InsertarMovContableResponse> InsertarMovContableAsync(InsertarMovContableRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SetContabToper", ReplyAction = "*")]
        SetContabToperResponse SetContabToper(SetContabToperRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://seven/SetContabToper", ReplyAction = "*")]
        System.Threading.Tasks.Task<SetContabToperResponse> SetContabToperAsync(SetContabToperRequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class InsertarCnMcontRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "InsertarCnMcont", Namespace = "http://seven/", Order = 0)]
        public InsertarCnMcontRequestBody Body;

        public InsertarCnMcontRequest()
        {
        }

        public InsertarCnMcontRequest(InsertarCnMcontRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class InsertarCnMcontRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public int emp_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public int top_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 2)]
        public int mco_nume;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public System.DateTime mco_fech;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public int mod_codi;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string arb_cods;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string mco_desc;

        public InsertarCnMcontRequestBody()
        {
        }

        public InsertarCnMcontRequestBody(int emp_codi, int top_codi, int mco_nume, System.DateTime mco_fech, int mod_codi, string arb_cods, string mco_desc)
        {
            this.emp_codi = emp_codi;
            this.top_codi = top_codi;
            this.mco_nume = mco_nume;
            this.mco_fech = mco_fech;
            this.mod_codi = mod_codi;
            this.arb_cods = arb_cods;
            this.mco_desc = mco_desc;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class InsertarCnMcontResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "InsertarCnMcontResponse", Namespace = "http://seven/", Order = 0)]
        public InsertarCnMcontResponseBody Body;

        public InsertarCnMcontResponse()
        {
        }

        public InsertarCnMcontResponse(InsertarCnMcontResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class InsertarCnMcontResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string InsertarCnMcontResult;

        public InsertarCnMcontResponseBody()
        {
        }

        public InsertarCnMcontResponseBody(string InsertarCnMcontResult)
        {
            this.InsertarCnMcontResult = InsertarCnMcontResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class InsertarCnDmconRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "InsertarCnDmcon", Namespace = "http://seven/", Order = 0)]
        public InsertarCnDmconRequestBody Body;

        public InsertarCnDmconRequest()
        {
        }

        public InsertarCnDmconRequest(InsertarCnDmconRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class InsertarCnDmconRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public int emp_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public int mco_cont;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string cue_codi;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string dmc_desc;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public double dmc_cant;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
        public double dmc_acti;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string dmc_refe;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 7)]
        public decimal dmc_vadb;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 8)]
        public decimal dmc_vacr;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9)]
        public decimal dmc_vaba;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string ter_coda;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string arb_codc;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string arb_codp;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string arb_coda;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public string arb_cods;

        public InsertarCnDmconRequestBody()
        {
        }

        public InsertarCnDmconRequestBody(int emp_codi, int mco_cont, string cue_codi, string dmc_desc, double dmc_cant, double dmc_acti, string dmc_refe, decimal dmc_vadb, decimal dmc_vacr, decimal dmc_vaba, string ter_coda, string arb_codc, string arb_codp, string arb_coda, string arb_cods)
        {
            this.emp_codi = emp_codi;
            this.mco_cont = mco_cont;
            this.cue_codi = cue_codi;
            this.dmc_desc = dmc_desc;
            this.dmc_cant = dmc_cant;
            this.dmc_acti = dmc_acti;
            this.dmc_refe = dmc_refe;
            this.dmc_vadb = dmc_vadb;
            this.dmc_vacr = dmc_vacr;
            this.dmc_vaba = dmc_vaba;
            this.ter_coda = ter_coda;
            this.arb_codc = arb_codc;
            this.arb_codp = arb_codp;
            this.arb_coda = arb_coda;
            this.arb_cods = arb_cods;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class InsertarCnDmconResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "InsertarCnDmconResponse", Namespace = "http://seven/", Order = 0)]
        public InsertarCnDmconResponseBody Body;

        public InsertarCnDmconResponse()
        {
        }

        public InsertarCnDmconResponse(InsertarCnDmconResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class InsertarCnDmconResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string InsertarCnDmconResult;

        public InsertarCnDmconResponseBody()
        {
        }

        public InsertarCnDmconResponseBody(string InsertarCnDmconResult)
        {
            this.InsertarCnDmconResult = InsertarCnDmconResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class AplicarCnMcontRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "AplicarCnMcont", Namespace = "http://seven/", Order = 0)]
        public AplicarCnMcontRequestBody Body;

        public AplicarCnMcontRequest()
        {
        }

        public AplicarCnMcontRequest(AplicarCnMcontRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class AplicarCnMcontRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public int emp_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public int mco_cont;

        public AplicarCnMcontRequestBody()
        {
        }

        public AplicarCnMcontRequestBody(int emp_codi, int mco_cont)
        {
            this.emp_codi = emp_codi;
            this.mco_cont = mco_cont;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class AplicarCnMcontResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "AplicarCnMcontResponse", Namespace = "http://seven/", Order = 0)]
        public AplicarCnMcontResponseBody Body;

        public AplicarCnMcontResponse()
        {
        }

        public AplicarCnMcontResponse(AplicarCnMcontResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class AplicarCnMcontResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string AplicarCnMcontResult;

        public AplicarCnMcontResponseBody()
        {
        }

        public AplicarCnMcontResponseBody(string AplicarCnMcontResult)
        {
            this.AplicarCnMcontResult = AplicarCnMcontResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RevertirCnMcontRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RevertirCnMcont", Namespace = "http://seven/", Order = 0)]
        public RevertirCnMcontRequestBody Body;

        public RevertirCnMcontRequest()
        {
        }

        public RevertirCnMcontRequest(RevertirCnMcontRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class RevertirCnMcontRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public int emp_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public int mco_cont;

        public RevertirCnMcontRequestBody()
        {
        }

        public RevertirCnMcontRequestBody(int emp_codi, int mco_cont)
        {
            this.emp_codi = emp_codi;
            this.mco_cont = mco_cont;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RevertirCnMcontResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RevertirCnMcontResponse", Namespace = "http://seven/", Order = 0)]
        public RevertirCnMcontResponseBody Body;

        public RevertirCnMcontResponse()
        {
        }

        public RevertirCnMcontResponse(RevertirCnMcontResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class RevertirCnMcontResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string RevertirCnMcontResult;

        public RevertirCnMcontResponseBody()
        {
        }

        public RevertirCnMcontResponseBody(string RevertirCnMcontResult)
        {
            this.RevertirCnMcontResult = RevertirCnMcontResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class AnularCnMcontRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "AnularCnMcont", Namespace = "http://seven/", Order = 0)]
        public AnularCnMcontRequestBody Body;

        public AnularCnMcontRequest()
        {
        }

        public AnularCnMcontRequest(AnularCnMcontRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class AnularCnMcontRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public int emp_codi;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 1)]
        public int mco_cont;

        public AnularCnMcontRequestBody()
        {
        }

        public AnularCnMcontRequestBody(int emp_codi, int mco_cont)
        {
            this.emp_codi = emp_codi;
            this.mco_cont = mco_cont;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class AnularCnMcontResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "AnularCnMcontResponse", Namespace = "http://seven/", Order = 0)]
        public AnularCnMcontResponseBody Body;

        public AnularCnMcontResponse()
        {
        }

        public AnularCnMcontResponse(AnularCnMcontResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class AnularCnMcontResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string AnularCnMcontResult;

        public AnularCnMcontResponseBody()
        {
        }

        public AnularCnMcontResponseBody(string AnularCnMcontResult)
        {
            this.AnularCnMcontResult = AnularCnMcontResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class InsertarMovContableRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "InsertarMovContable", Namespace = "http://seven/", Order = 0)]
        public InsertarMovContableRequestBody Body;

        public InsertarMovContableRequest()
        {
        }

        public InsertarMovContableRequest(InsertarMovContableRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class InsertarMovContableRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public TSCnMcont pContable;

        public InsertarMovContableRequestBody()
        {
        }

        public InsertarMovContableRequestBody(TSCnMcont pContable)
        {
            this.pContable = pContable;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class InsertarMovContableResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "InsertarMovContableResponse", Namespace = "http://seven/", Order = 0)]
        public InsertarMovContableResponseBody Body;

        public InsertarMovContableResponse()
        {
        }

        public InsertarMovContableResponse(InsertarMovContableResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class InsertarMovContableResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string InsertarMovContableResult;

        public InsertarMovContableResponseBody()
        {
        }

        public InsertarMovContableResponseBody(string InsertarMovContableResult)
        {
            this.InsertarMovContableResult = InsertarMovContableResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SetContabToperRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SetContabToper", Namespace = "http://seven/", Order = 0)]
        public SetContabToperRequestBody Body;

        public SetContabToperRequest()
        {
        }

        public SetContabToperRequest(SetContabToperRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class SetContabToperRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public TToper[] pInput;

        public SetContabToperRequestBody()
        {
        }

        public SetContabToperRequestBody(TToper[] pInput)
        {
            this.pInput = pInput;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SetContabToperResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SetContabToperResponse", Namespace = "http://seven/", Order = 0)]
        public SetContabToperResponseBody Body;

        public SetContabToperResponse()
        {
        }

        public SetContabToperResponse(SetContabToperResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://seven/")]
    public partial class SetContabToperResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public TRetToper[] SetContabToperResult;

        public SetContabToperResponseBody()
        {
        }

        public SetContabToperResponseBody(TRetToper[] SetContabToperResult)
        {
            this.SetContabToperResult = SetContabToperResult;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface SCNMCONTSoapChannel : SCNMCONTSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class SCNMCONTSoapClient : System.ServiceModel.ClientBase<SCNMCONTSoap>, SCNMCONTSoap
    {

        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public SCNMCONTSoapClient(EndpointConfiguration endpointConfiguration) :
                base(SCNMCONTSoapClient.GetBindingForEndpoint(endpointConfiguration), SCNMCONTSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SCNMCONTSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(SCNMCONTSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SCNMCONTSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(SCNMCONTSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public SCNMCONTSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public InsertarCnMcontResponse InsertarCnMcont(InsertarCnMcontRequest request)
        {
            return base.Channel.InsertarCnMcont(request);
        }

        public System.Threading.Tasks.Task<InsertarCnMcontResponse> InsertarCnMcontAsync(InsertarCnMcontRequest request)
        {
            return base.Channel.InsertarCnMcontAsync(request);
        }

        public InsertarCnDmconResponse InsertarCnDmcon(InsertarCnDmconRequest request)
        {
            return base.Channel.InsertarCnDmcon(request);
        }

        public System.Threading.Tasks.Task<InsertarCnDmconResponse> InsertarCnDmconAsync(InsertarCnDmconRequest request)
        {
            return base.Channel.InsertarCnDmconAsync(request);
        }

        public AplicarCnMcontResponse AplicarCnMcont(AplicarCnMcontRequest request)
        {
            return base.Channel.AplicarCnMcont(request);
        }

        public System.Threading.Tasks.Task<AplicarCnMcontResponse> AplicarCnMcontAsync(AplicarCnMcontRequest request)
        {
            return base.Channel.AplicarCnMcontAsync(request);
        }

        public RevertirCnMcontResponse RevertirCnMcont(RevertirCnMcontRequest request)
        {
            return base.Channel.RevertirCnMcont(request);
        }

        public System.Threading.Tasks.Task<RevertirCnMcontResponse> RevertirCnMcontAsync(RevertirCnMcontRequest request)
        {
            return base.Channel.RevertirCnMcontAsync(request);
        }

        public AnularCnMcontResponse AnularCnMcont(AnularCnMcontRequest request)
        {
            return base.Channel.AnularCnMcont(request);
        }

        public System.Threading.Tasks.Task<AnularCnMcontResponse> AnularCnMcontAsync(AnularCnMcontRequest request)
        {
            return base.Channel.AnularCnMcontAsync(request);
        }

        public InsertarMovContableResponse InsertarMovContable(InsertarMovContableRequest request)
        {
            return base.Channel.InsertarMovContable(request);
        }

        public System.Threading.Tasks.Task<InsertarMovContableResponse> InsertarMovContableAsync(InsertarMovContableRequest request)
        {
            return base.Channel.InsertarMovContableAsync(request);
        }

        public SetContabToperResponse SetContabToper(SetContabToperRequest request)
        {
            return base.Channel.SetContabToper(request);
        }

        public System.Threading.Tasks.Task<SetContabToperResponse> SetContabToperAsync(SetContabToperRequest request)
        {
            return base.Channel.SetContabToperAsync(request);
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
            if ((endpointConfiguration == EndpointConfiguration.SCNMCONTSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.SCNMCONTSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.SCNMCONTSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSMovContable/SCnMcont.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.SCNMCONTSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://cpvmp-34xp/Seven/WSMovContable/SCnMcont.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            SCNMCONTSoap,

            SCNMCONTSoap12,
        }
    }
}
