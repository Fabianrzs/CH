using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GA2.Application.Dto.Credito.TU.Responses
{
    public class ResponseErrorTu
    {

        [XmlRoot(ElementName = "Authentication")]
        public class Authentication
        {
            [XmlElement(ElementName = "Status")]
            public string Status { get; set; }
            [XmlElement(ElementName = "Token")]
            public string Token { get; set; }
        }

        [XmlRoot(ElementName = "ResponseInfo")]
        public class ResponseInfo
        {
            [XmlElement(ElementName = "ApplicationId")]
            public string ApplicationId { get; set; }
            [XmlElement(ElementName = "SolutionSetInstanceId")]
            public string SolutionSetInstanceId { get; set; }
            [XmlElement(ElementName = "CurrentQueue")]
            public string CurrentQueue { get; set; }
        }

        [XmlRoot(ElementName = "Derived")]
        public class Derived
        {
            [XmlElement(ElementName = "IdType")]
            public string IdType { get; set; }
        }

        [XmlRoot(ElementName = "Attributes")]
        public class Attributes
        {
            [XmlElement(ElementName = "Derived")]
            public Derived Derived { get; set; }
        }

        [XmlRoot(ElementName = "VelocityCheckStatus")]
        public class VelocityCheckStatus
        {
            [XmlElement(ElementName = "Current")]
            public string Current { get; set; }
            [XmlElement(ElementName = "Outcome")]
            public string Outcome { get; set; }
        }

        [XmlRoot(ElementName = "Reason")]
        public class Reason
        {
            [XmlElement(ElementName = "Applicant")]
            public string Applicant { get; set; }
            [XmlElement(ElementName = "Code")]
            public string Code { get; set; }
            [XmlElement(ElementName = "Type")]
            public string Type { get; set; }
            [XmlElement(ElementName = "Description")]
            public string Description { get; set; }
        }

        [XmlRoot(ElementName = "VelocityCheckReasons")]
        public class VelocityCheckReasons
        {
            [XmlElement(ElementName = "Reason")]
            public Reason Reason { get; set; }
        }

        [XmlRoot(ElementName = "VelocityCheckData")]
        public class VelocityCheckData
        {
            [XmlElement(ElementName = "VelocityCheckReasons")]
            public VelocityCheckReasons VelocityCheckReasons { get; set; }
            [XmlElement(ElementName = "RiskLevel")]
            public string RiskLevel { get; set; }
            [XmlElement(ElementName = "CheckOutput")]
            public string CheckOutput { get; set; }
            [XmlElement(ElementName = "Key1")]
            public string Key1 { get; set; }
            [XmlElement(ElementName = "Key2")]
            public string Key2 { get; set; }
            [XmlElement(ElementName = "Key3")]
            public string Key3 { get; set; }
            [XmlElement(ElementName = "Key4")]
            public string Key4 { get; set; }
            [XmlElement(ElementName = "CustomKey1")]
            public string CustomKey1 { get; set; }
            [XmlElement(ElementName = "CustomKey2")]
            public string CustomKey2 { get; set; }
            [XmlElement(ElementName = "RequestCount")]
            public string RequestCount { get; set; }
        }

        [XmlRoot(ElementName = "Applicant")]
        public class Applicant
        {
            [XmlElement(ElementName = "MonthlyIncome")]
            public string MonthlyIncome { get; set; }
            [XmlElement(ElementName = "Occupation")]
            public string Occupation { get; set; }
            [XmlElement(ElementName = "IdExpeditionDate")]
            public string IdExpeditionDate { get; set; }
            [XmlElement(ElementName = "RecentPhoneNumber")]
            public string RecentPhoneNumber { get; set; }
            [XmlElement(ElementName = "LastName")]
            public string LastName { get; set; }
            [XmlElement(ElementName = "IdNumber")]
            public string IdNumber { get; set; }
            [XmlElement(ElementName = "IdType")]
            public string IdType { get; set; }
            [XmlElement(ElementName = "TipoScore")]
            public string TipoScore { get; set; }
            [XmlElement(ElementName = "Attributes")]
            public Attributes Attributes { get; set; }
            [XmlElement(ElementName = "VelocityCheckStatus")]
            public VelocityCheckStatus VelocityCheckStatus { get; set; }
            [XmlElement(ElementName = "VelocityCheckData")]
            public VelocityCheckData VelocityCheckData { get; set; }
            [XmlElement(ElementName = "CapacidadEndeudamiento")]
            public string CapacidadEndeudamiento { get; set; }
            [XmlElement(ElementName = "FullName")]
            public string FullName { get; set; }
            [XmlElement(ElementName = "Reasons")]
            public string Reasons { get; set; }
        }

        [XmlRoot(ElementName = "Applicants")]
        public class Applicants
        {
            [XmlElement(ElementName = "Applicant")]
            public Applicant Applicant { get; set; }
        }

        [XmlRoot(ElementName = "Field")]
        public class Field
        {
            [XmlElement(ElementName = "Applicants")]
            public Applicants Applicants { get; set; }
            [XmlAttribute(AttributeName = "key")]
            public string Key { get; set; }
            [XmlElement(ElementName = "ApplicationData")]
            public ApplicationData ApplicationData { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "Milestone")]
        public class Milestone
        {
            [XmlElement(ElementName = "Step")]
            public List<string> Step { get; set; }
        }

        [XmlRoot(ElementName = "Step")]
        public class Step
        {
            [XmlElement(ElementName = "Name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "Duration")]
            public string Duration { get; set; }
        }

        [XmlRoot(ElementName = "DTTrail")]
        public class DTTrail
        {
            [XmlElement(ElementName = "Step")]
            public List<Step> Step { get; set; }
        }

        [XmlRoot(ElementName = "DinamicSelector2")]
        public class DinamicSelector2
        {
            [XmlElement(ElementName = "PreviousGroup")]
            public string PreviousGroup { get; set; }
            [XmlElement(ElementName = "PreviousStep")]
            public string PreviousStep { get; set; }
            [XmlElement(ElementName = "NextGroup")]
            public string NextGroup { get; set; }
            [XmlElement(ElementName = "NextStep")]
            public string NextStep { get; set; }
            [XmlElement(ElementName = "TypeofCondition")]
            public string TypeofCondition { get; set; }
            [XmlElement(ElementName = "BoolCondition")]
            public string BoolCondition { get; set; }
            [XmlElement(ElementName = "Variable1")]
            public string Variable1 { get; set; }
            [XmlElement(ElementName = "Condition")]
            public string Condition { get; set; }
            [XmlElement(ElementName = "Variable2")]
            public string Variable2 { get; set; }
        }

        [XmlRoot(ElementName = "DinamicSelector2s")]
        public class DinamicSelector2s
        {
            [XmlElement(ElementName = "DinamicSelector2")]
            public List<DinamicSelector2> DinamicSelector2 { get; set; }
        }

        [XmlRoot(ElementName = "DinamicSelector")]
        public class DinamicSelector
        {
            [XmlElement(ElementName = "DinamicSelector2s")]
            public DinamicSelector2s DinamicSelector2s { get; set; }
        }

        [XmlRoot(ElementName = "ApplicationData")]
        public class ApplicationData
        {
            [XmlElement(ElementName = "Product")]
            public string Product { get; set; }
            [XmlElement(ElementName = "User")]
            public string User { get; set; }
            [XmlElement(ElementName = "BusinessUnitId")]
            public string BusinessUnitId { get; set; }
            [XmlElement(ElementName = "ApplicationId")]
            public string ApplicationId { get; set; }
            [XmlElement(ElementName = "SolutionSetId")]
            public string SolutionSetId { get; set; }
            [XmlElement(ElementName = "EnvironmentTypeId")]
            public string EnvironmentTypeId { get; set; }
            [XmlElement(ElementName = "EnvironmentType")]
            public string EnvironmentType { get; set; }
            [XmlElement(ElementName = "SkipVelocityCheckFlag")]
            public string SkipVelocityCheckFlag { get; set; }
            [XmlElement(ElementName = "SkipDsExpertoCOLFlag")]
            public string SkipDsExpertoCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipDsExpertoEntidadCOLFlag")]
            public string SkipDsExpertoEntidadCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipDSUbicaCOLFlag")]
            public string SkipDSUbicaCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipLookupInternalFlag")]
            public string SkipLookupInternalFlag { get; set; }
            [XmlElement(ElementName = "SkipCriteriaPostBureauCOLFlag")]
            public string SkipCriteriaPostBureauCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipCreditVisionFlag")]
            public string SkipCreditVisionFlag { get; set; }
            [XmlElement(ElementName = "SkipIncomeValidatorFlag")]
            public string SkipIncomeValidatorFlag { get; set; }
            [XmlElement(ElementName = "SkipAffordabilityCalcCOLFlag")]
            public string SkipAffordabilityCalcCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipBillingFlag")]
            public string SkipBillingFlag { get; set; }
            [XmlElement(ElementName = "SkipDSTUBureauCOLFlag")]
            public string SkipDSTUBureauCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipFlagFacturacionOnline")]
            public string SkipFlagFacturacionOnline { get; set; }
            [XmlElement(ElementName = "CreateDate")]
            public string CreateDate { get; set; }
            [XmlElement(ElementName = "FlowType")]
            public string FlowType { get; set; }
            [XmlElement(ElementName = "SMMLV")]
            public string SMMLV { get; set; }
            [XmlElement(ElementName = "FactorTDC")]
            public string FactorTDC { get; set; }
            [XmlElement(ElementName = "RotatingFactor")]
            public string RotatingFactor { get; set; }
            [XmlElement(ElementName = "EndPercentageMaximum")]
            public string EndPercentageMaximum { get; set; }
            [XmlElement(ElementName = "MinimumFee")]
            public string MinimumFee { get; set; }
            [XmlElement(ElementName = "MaximumUtilizationPercentage")]
            public string MaximumUtilizationPercentage { get; set; }
            [XmlElement(ElementName = "FeeCalculation")]
            public string FeeCalculation { get; set; }
            [XmlElement(ElementName = "TypeofCuota")]
            public string TypeofCuota { get; set; }
            [XmlElement(ElementName = "MaxNumberPendingRules")]
            public string MaxNumberPendingRules { get; set; }
            [XmlElement(ElementName = "ReportDays")]
            public string ReportDays { get; set; }
            [XmlElement(ElementName = "TipoScore")]
            public string TipoScore { get; set; }
            [XmlElement(ElementName = "Milestone")]
            public Milestone Milestone { get; set; }
            [XmlElement(ElementName = "Start")]
            public string Start { get; set; }
            [XmlElement(ElementName = "InputValReasonCodes")]
            public string InputValReasonCodes { get; set; }
            [XmlElement(ElementName = "DTTrail")]
            public DTTrail DTTrail { get; set; }
            [XmlElement(ElementName = "DinamicSelector")]
            public DinamicSelector DinamicSelector { get; set; }
            [XmlElement(ElementName = "Permutacion")]
            public string Permutacion { get; set; }
            [XmlElement(ElementName = "VelocityCheckReasons")]
            public VelocityCheckReasons VelocityCheckReasons { get; set; }
            [XmlElement(ElementName = "PolicyPostBureauComplete")]
            public string PolicyPostBureauComplete { get; set; }
            [XmlElement(ElementName = "Decision")]
            public string Decision { get; set; }
            [XmlElement(ElementName = "DecisionHomologada")]
            public string DecisionHomologada { get; set; }
            [XmlElement(ElementName = "Test")]
            public List<string> Test { get; set; }
            [XmlElement(ElementName = "XSLFile")]
            public string XSLFile { get; set; }
            [XmlElement(ElementName = "HTMLFile")]
            public string HTMLFile { get; set; }
            [XmlElement(ElementName = "XMLFile")]
            public string XMLFile { get; set; }
            [XmlElement(ElementName = "GenerarReporte")]
            public string GenerarReporte { get; set; }
            [XmlElement(ElementName = "CreateHour")]
            public string CreateHour { get; set; }
            [XmlElement(ElementName = "UltimaEtapaFlujo")]
            public string UltimaEtapaFlujo { get; set; }
        }

        [XmlRoot(ElementName = "ContextData")]
        public class ContextData
        {
            [XmlElement(ElementName = "Field")]
            public List<Field> Field { get; set; }
        }

        [XmlRoot(ElementName = "DCResponse")]
        public class DCResponse
        {
            [XmlElement(ElementName = "Status")]
            public string Status { get; set; }
            [XmlElement(ElementName = "Authentication")]
            public Authentication Authentication { get; set; }
            [XmlElement(ElementName = "ResponseInfo")]
            public ResponseInfo ResponseInfo { get; set; }
            [XmlElement(ElementName = "ContextData")]
            public ContextData ContextData { get; set; }
        }

    }

}

