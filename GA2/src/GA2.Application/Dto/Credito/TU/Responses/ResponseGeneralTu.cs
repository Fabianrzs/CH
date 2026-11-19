using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GA2.Application.Dto.Credito.TU.Responses
{
    public class ResponseGeneralTu
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

        [XmlRoot(ElementName = "Reason")]
        public class Reason
        {
            [XmlElement(ElementName = "Description")]
            public string Description { get; set; }
            [XmlElement(ElementName = "Type")]
            public string Type { get; set; }
            [XmlElement(ElementName = "Code")]
            public string Code { get; set; }
            [XmlElement(ElementName = "Applicant")]
            public string Applicant { get; set; }
        }

        [XmlRoot(ElementName = "PostBureauReasons")]
        public class PostBureauReasons
        {
            [XmlElement(ElementName = "Reason")]
            public Reason Reason { get; set; }
        }

        [XmlRoot(ElementName = "CriteriaPostBureauCOLData")]
        public class CriteriaPostBureauCOLData
        {
            [XmlElement(ElementName = "PostBureauReasons")]
            public PostBureauReasons PostBureauReasons { get; set; }
            [XmlElement(ElementName = "RiskAssignment")]
            public string RiskAssignment { get; set; }
            [XmlElement(ElementName = "WhiteList")]
            public string WhiteList { get; set; }
            [XmlElement(ElementName = "PostBureauOutput")]
            public string PostBureauOutput { get; set; }
        }

        [XmlRoot(ElementName = "CriteriaPostBureauCOLStatus")]
        public class CriteriaPostBureauCOLStatus
        {
            [XmlElement(ElementName = "IsSuccess")]
            public string IsSuccess { get; set; }
            [XmlElement(ElementName = "Outcome")]
            public string Outcome { get; set; }
            [XmlElement(ElementName = "Current")]
            public string Current { get; set; }
        }

        [XmlRoot(ElementName = "Reasons")]
        public class Reasons
        {
            [XmlElement(ElementName = "Reason")]
            public Reason Reason { get; set; }
        }

        [XmlRoot(ElementName = "DSCreditVisionCOLStatus")]
        public class DSCreditVisionCOLStatus
        {
            [XmlElement(ElementName = "IsSuccess")]
            public string IsSuccess { get; set; }
        }

        [XmlRoot(ElementName = "DSCreditVisionCOLData")]
        public class DSCreditVisionCOLData
        {
            [XmlElement(ElementName = "Score")]
            public string Score { get; set; }
            [XmlElement(ElementName = "CV_LINK_TEL")]
            public string CV_LINK_TEL { get; set; }
            [XmlElement(ElementName = "CV_LINK_FIN")]
            public string CV_LINK_FIN { get; set; }
            [XmlElement(ElementName = "CV_LINK_GROUP")]
            public string CV_LINK_GROUP { get; set; }
            [XmlElement(ElementName = "BALMAG03")]
            public string BALMAG03 { get; set; }
            [XmlElement(ElementName = "BALMAG01")]
            public string BALMAG01 { get; set; }
            [XmlElement(ElementName = "identificadorLinea")]
            public string IdentificadorLinea { get; set; }
            [XmlElement(ElementName = "tipoIdentificacion")]
            public string TipoIdentificacion { get; set; }
            [XmlElement(ElementName = "codigoTipoIndentificacion")]
            public string CodigoTipoIndentificacion { get; set; }
            [XmlElement(ElementName = "nombreTitular")]
            public string NombreTitular { get; set; }
            [XmlElement(ElementName = "nombreCiiu")]
            public string NombreCiiu { get; set; }
            [XmlElement(ElementName = "lugarExpedicion")]
            public string LugarExpedicion { get; set; }
            [XmlElement(ElementName = "fechaExpedicion")]
            public string FechaExpedicion { get; set; }
            [XmlElement(ElementName = "estado")]
            public string Estado { get; set; }
            [XmlElement(ElementName = "rangoEdad")]
            public string RangoEdad { get; set; }
            [XmlElement(ElementName = "codigoCiiu")]
            public string CodigoCiiu { get; set; }
            [XmlElement(ElementName = "codigoDepartamento")]
            public string CodigoDepartamento { get; set; }
            [XmlElement(ElementName = "codigoMunicipio")]
            public string CodigoMunicipio { get; set; }
            [XmlElement(ElementName = "fecha")]
            public string Fecha { get; set; }
            [XmlElement(ElementName = "hora")]
            public string Hora { get; set; }
            [XmlElement(ElementName = "entidad")]
            public string Entidad { get; set; }
            [XmlElement(ElementName = "policyId")]
            public string PolicyId { get; set; }
            [XmlElement(ElementName = "transactionId")]
            public string TransactionId { get; set; }
            [XmlElement(ElementName = "resultadoOperacion")]
            public string ResultadoOperacion { get; set; }
            [XmlElement(ElementName = "xmlRawResponse")]
            public string XmlRawResponse { get; set; }
            [XmlElement(ElementName = "Request")]
            public string Request { get; set; }
            [XmlElement(ElementName = "Response")]
            public string Response { get; set; }
        }

        [XmlRoot(ElementName = "DsExpertoReasonsCode")]
        public class DsExpertoReasonsCode
        {
            [XmlElement(ElementName = "Reason")]
            public Reason Reason { get; set; }
        }

        [XmlRoot(ElementName = "DsExpertoCOLData")]
        public class DsExpertoCOLData
        {
            [XmlElement(ElementName = "DsExpertoReasonsCode")]
            public DsExpertoReasonsCode DsExpertoReasonsCode { get; set; }
            [XmlElement(ElementName = "RetryCount")]
            public string RetryCount { get; set; }
        }

        [XmlRoot(ElementName = "DsExpertoCOLStatus")]
        public class DsExpertoCOLStatus
        {
            [XmlElement(ElementName = "ErrorMessage")]
            public string ErrorMessage { get; set; }
            [XmlElement(ElementName = "ErrorCode")]
            public string ErrorCode { get; set; }
            [XmlElement(ElementName = "IsSuccess")]
            public string IsSuccess { get; set; }
            [XmlElement(ElementName = "Trail")]
            public string Trail { get; set; }
        }

        [XmlRoot(ElementName = "DsExpertoCOL")]
        public class DsExpertoCOL
        {
            [XmlElement(ElementName = "DsExpertoCOLRequest")]
            public string DsExpertoCOLRequest { get; set; }
            [XmlElement(ElementName = "DsExpertoCOLStatus")]
            public DsExpertoCOLStatus DsExpertoCOLStatus { get; set; }
            [XmlElement(ElementName = "DsExpertoCOLData")]
            public DsExpertoCOLData DsExpertoCOLData { get; set; }
        }

        [XmlRoot(ElementName = "IDMReasonCodes")]
        public class IDMReasonCodes
        {
            [XmlElement(ElementName = "Reason")]
            public Reason Reason { get; set; }
        }

        [XmlRoot(ElementName = "DSUbicaCOLStatus")]
        public class DSUbicaCOLStatus
        {
            [XmlElement(ElementName = "Outcome")]
            public string Outcome { get; set; }
            [XmlElement(ElementName = "ErrorMessage")]
            public string ErrorMessage { get; set; }
            [XmlElement(ElementName = "ErrorCode")]
            public string ErrorCode { get; set; }
            [XmlElement(ElementName = "IsSuccess")]
            public string IsSuccess { get; set; }
        }

        [XmlRoot(ElementName = "IDVReasonsCode")]
        public class IDVReasonsCode
        {
            [XmlElement(ElementName = "Reason")]
            public Reason Reason { get; set; }
        }

        [XmlRoot(ElementName = "Matching")]
        public class Matching
        {
            [XmlElement(ElementName = "Address")]
            public string Address { get; set; }
            [XmlElement(ElementName = "Telephone")]
            public string Telephone { get; set; }
            [XmlElement(ElementName = "LastName")]
            public string LastName { get; set; }
            [XmlElement(ElementName = "IssuePlace")]
            public string IssuePlace { get; set; }
            [XmlElement(ElementName = "IssueDate")]
            public string IssueDate { get; set; }
        }

        [XmlRoot(ElementName = "MostRecentAddresses")]
        public class MostRecentAddresses
        {
            [XmlElement(ElementName = "Type")]
            public string Type { get; set; }
            [XmlElement(ElementName = "City")]
            public string City { get; set; }
            [XmlElement(ElementName = "Address")]
            public string Address { get; set; }
            [XmlElement(ElementName = "LastDate")]
            public string LastDate { get; set; }
            [XmlElement(ElementName = "FirstDate")]
            public string FirstDate { get; set; }
            [XmlElement(ElementName = "Sector")]
            public string Sector { get; set; }
        }

        [XmlRoot(ElementName = "MostRecentPhones")]
        public class MostRecentPhones
        {
            [XmlElement(ElementName = "PhoneFromBD")]
            public string PhoneFromBD { get; set; }
            [XmlElement(ElementName = "TelephoneNumber")]
            public string TelephoneNumber { get; set; }
            [XmlElement(ElementName = "TelephonePrefix")]
            public string TelephonePrefix { get; set; }
            [XmlElement(ElementName = "TelephoneType")]
            public string TelephoneType { get; set; }
        }

        [XmlRoot(ElementName = "_Attributes_CIFIN")]
        public class _Attributes_CIFIN
        {
            [XmlElement(ElementName = "archivo")]
            public string Archivo { get; set; }
        }

        [XmlRoot(ElementName = "_Attributes_Direccion")]
        public class _Attributes_Direccion
        {
            [XmlElement(ElementName = "pos")]
            public string Pos { get; set; }
        }

        [XmlRoot(ElementName = "Direccion")]
        public class Direccion
        {
            [XmlElement(ElementName = "_Attributes_Direccion")]
            public _Attributes_Direccion _Attributes_Direccion { get; set; }
            [XmlElement(ElementName = "DirPos")]
            public string DirPos { get; set; }
            [XmlElement(ElementName = "NoReportes")]
            public string NoReportes { get; set; }
            [XmlElement(ElementName = "ProductoActivo")]
            public string ProductoActivo { get; set; }
            [XmlElement(ElementName = "TipoUbicacion")]
            public string TipoUbicacion { get; set; }
            [XmlElement(ElementName = "SectorEconomico")]
            public string SectorEconomico { get; set; }
            [XmlElement(ElementName = "PrimerReporte")]
            public string PrimerReporte { get; set; }
            [XmlElement(ElementName = "UltimoReporte")]
            public string UltimoReporte { get; set; }
            [XmlElement(ElementName = "Direccion")]
            public string Direccionn { get; set; }
            [XmlElement(ElementName = "Ciudad")]
            public string Ciudad { get; set; }
        }

        [XmlRoot(ElementName = "Direcciones")]
        public class Direcciones
        {
            [XmlElement(ElementName = "Direccion")]
            public List<Direccion> Direccion { get; set; }
        }

        [XmlRoot(ElementName = "_Attributes_Telefono")]
        public class _Attributes_Telefono
        {
            [XmlElement(ElementName = "pos")]
            public string Pos { get; set; }
        }

        [XmlRoot(ElementName = "Telefono")]
        public class Telefono
        {
            [XmlElement(ElementName = "_Attributes_Telefono")]
            public _Attributes_Telefono _Attributes_Telefono { get; set; }
            [XmlElement(ElementName = "TelPos")]
            public string TelPos { get; set; }
            [XmlElement(ElementName = "NoReportes")]
            public string NoReportes { get; set; }
            [XmlElement(ElementName = "ProductoActivo")]
            public string ProductoActivo { get; set; }
            [XmlElement(ElementName = "TipoUbicacion")]
            public string TipoUbicacion { get; set; }
            [XmlElement(ElementName = "SectorEconomico")]
            public string SectorEconomico { get; set; }
            [XmlElement(ElementName = "PrimerReporte")]
            public string PrimerReporte { get; set; }
            [XmlElement(ElementName = "UltimoReporte")]
            public string UltimoReporte { get; set; }
            [XmlElement(ElementName = "Prefijo")]
            public string Prefijo { get; set; }
            [XmlElement(ElementName = "Telefono")]
            public string Telefonoo { get; set; }
            [XmlElement(ElementName = "Ciudad")]
            public string Ciudad { get; set; }
        }

        [XmlRoot(ElementName = "Telefonos")]
        public class Telefonos
        {
            [XmlElement(ElementName = "Telefono")]
            public List<Telefono> Telefono { get; set; }
        }

        [XmlRoot(ElementName = "_Attributes_Celular")]
        public class _Attributes_Celular
        {
            [XmlElement(ElementName = "pos")]
            public string Pos { get; set; }
        }

        [XmlRoot(ElementName = "Celular")]
        public class Celular
        {
            [XmlElement(ElementName = "_Attributes_Celular")]
            public _Attributes_Celular _Attributes_Celular { get; set; }
            [XmlElement(ElementName = "CelPos")]
            public string CelPos { get; set; }
            [XmlElement(ElementName = "NoReportes")]
            public string NoReportes { get; set; }
            [XmlElement(ElementName = "ProductoActivo")]
            public string ProductoActivo { get; set; }
            [XmlElement(ElementName = "SectorEconomico")]
            public string SectorEconomico { get; set; }
            [XmlElement(ElementName = "PrimerReporte")]
            public string PrimerReporte { get; set; }
            [XmlElement(ElementName = "UltimoReporte")]
            public string UltimoReporte { get; set; }
            [XmlElement(ElementName = "Celular")]
            public string Celularr { get; set; }
        }

        [XmlRoot(ElementName = "Celulares")]
        public class Celulares
        {
            [XmlElement(ElementName = "Celular")]
            public List<Celular> Celular { get; set; }
        }

        [XmlRoot(ElementName = "_Attributes_Mail")]
        public class _Attributes_Mail
        {
            [XmlElement(ElementName = "pos")]
            public string Pos { get; set; }
        }

        [XmlRoot(ElementName = "Mail")]
        public class Mail
        {
            [XmlElement(ElementName = "_Attributes_Mail")]
            public _Attributes_Mail _Attributes_Mail { get; set; }
            [XmlElement(ElementName = "NoReportes")]
            public string NoReportes { get; set; }
            [XmlElement(ElementName = "PrimerReporte")]
            public string PrimerReporte { get; set; }
            [XmlElement(ElementName = "UltimoReporte")]
            public string UltimoReporte { get; set; }
            [XmlElement(ElementName = "Correo")]
            public string Correo { get; set; }
        }

        [XmlRoot(ElementName = "Mails")]
        public class Mails
        {
            [XmlElement(ElementName = "Mail")]
            public List<Mail> Mail { get; set; }
        }

        [XmlRoot(ElementName = "UbicaPlusCifin")]
        public class UbicaPlusCifin
        {
            [XmlElement(ElementName = "GeneroTercero")]
            public string GeneroTercero { get; set; }
            [XmlElement(ElementName = "Direcciones")]
            public Direcciones Direcciones { get; set; }
            [XmlElement(ElementName = "Telefonos")]
            public Telefonos Telefonos { get; set; }
            [XmlElement(ElementName = "Celulares")]
            public Celulares Celulares { get; set; }
            [XmlElement(ElementName = "Mails")]
            public Mails Mails { get; set; }
        }

        [XmlRoot(ElementName = "Tercero")]
        public class Tercero
        {
            [XmlElement(ElementName = "EstadoDocumento")]
            public string EstadoDocumento { get; set; }
            [XmlElement(ElementName = "IdentificadorLinea")]
            public string IdentificadorLinea { get; set; }
            [XmlElement(ElementName = "TipoIdentificacion")]
            public string TipoIdentificacion { get; set; }
            [XmlElement(ElementName = "CodigoTipoIndentificacion")]
            public string CodigoTipoIndentificacion { get; set; }
            [XmlElement(ElementName = "NumeroIdentificacion")]
            public string NumeroIdentificacion { get; set; }
            [XmlElement(ElementName = "Nombre1")]
            public string Nombre1 { get; set; }
            [XmlElement(ElementName = "Nombre2")]
            public string Nombre2 { get; set; }
            [XmlElement(ElementName = "Apellido1")]
            public string Apellido1 { get; set; }
            [XmlElement(ElementName = "Apellido2")]
            public string Apellido2 { get; set; }
            [XmlElement(ElementName = "NombreTitular")]
            public string NombreTitular { get; set; }
            [XmlElement(ElementName = "NombreCiiu")]
            public string NombreCiiu { get; set; }
            [XmlElement(ElementName = "LugarExpedicion")]
            public string LugarExpedicion { get; set; }
            [XmlElement(ElementName = "FechaExpedicion")]
            public string FechaExpedicion { get; set; }
            [XmlElement(ElementName = "Estado")]
            public string Estado { get; set; }
            [XmlElement(ElementName = "NumeroInforme")]
            public string NumeroInforme { get; set; }
            [XmlElement(ElementName = "EstadoTitular")]
            public string EstadoTitular { get; set; }
            [XmlElement(ElementName = "RangoEdad")]
            public string RangoEdad { get; set; }
            [XmlElement(ElementName = "CodigoCiiu")]
            public string CodigoCiiu { get; set; }
            [XmlElement(ElementName = "CodigoDepartamento")]
            public string CodigoDepartamento { get; set; }
            [XmlElement(ElementName = "CodigoMunicipio")]
            public string CodigoMunicipio { get; set; }
            [XmlElement(ElementName = "Fecha")]
            public string Fecha { get; set; }
            [XmlElement(ElementName = "Hora")]
            public string Hora { get; set; }
            [XmlElement(ElementName = "Entidad")]
            public string Entidad { get; set; }
            [XmlElement(ElementName = "RespuestaConsulta")]
            public string RespuestaConsulta { get; set; }
            [XmlElement(ElementName = "UbicaPlusCifin")]
            public UbicaPlusCifin UbicaPlusCifin { get; set; }
            [XmlElement(ElementName = "HuellaConsulta")]
            public HuellaConsulta HuellaConsulta { get; set; }
            [XmlElement(ElementName = "Endeudamiento")]
            public Endeudamiento Endeudamiento { get; set; }
            [XmlElement(ElementName = "SectorRealExtinguidas")]
            public SectorRealExtinguidas SectorRealExtinguidas { get; set; }
            [XmlElement(ElementName = "SectorRealAlDia")]
            public SectorRealAlDia SectorRealAlDia { get; set; }
            [XmlElement(ElementName = "SectorFinancieroExtinguidas")]
            public SectorFinancieroExtinguidas SectorFinancieroExtinguidas { get; set; }
            [XmlElement(ElementName = "SectorFinancieroAlDia")]
            public SectorFinancieroAlDia SectorFinancieroAlDia { get; set; }
            [XmlElement(ElementName = "CuentasVigentes")]
            public CuentasVigentes CuentasVigentes { get; set; }
            [XmlElement(ElementName = "Consolidado")]
            public Consolidado Consolidado { get; set; }
        }

        [XmlRoot(ElementName = "Response")]
        public class Response
        {
            [XmlElement(ElementName = "_Attributes_CIFIN")]
            public _Attributes_CIFIN _Attributes_CIFIN { get; set; }
            [XmlElement(ElementName = "Tercero")]
            public Tercero Tercero { get; set; }
        }

        [XmlRoot(ElementName = "DSUbicaCOLData")]
        public class DSUbicaCOLData
        {
            [XmlElement(ElementName = "IDVReasonsCode")]
            public IDVReasonsCode IDVReasonsCode { get; set; }
            [XmlElement(ElementName = "RiskLevel")]
            public string RiskLevel { get; set; }
            [XmlElement(ElementName = "IDVOutput")]
            public string IDVOutput { get; set; }
            [XmlElement(ElementName = "porcentajeMatchApellido")]
            public string PorcentajeMatchApellido { get; set; }
            [XmlElement(ElementName = "Matching")]
            public Matching Matching { get; set; }
            [XmlElement(ElementName = "MostRecentAddresses")]
            public List<MostRecentAddresses> MostRecentAddresses { get; set; }
            [XmlElement(ElementName = "MostRecentPhones")]
            public List<MostRecentPhones> MostRecentPhones { get; set; }
            [XmlElement(ElementName = "IdType")]
            public string IdType { get; set; }
            [XmlElement(ElementName = "LastName")]
            public string LastName { get; set; }
            [XmlElement(ElementName = "IssuePlace")]
            public string IssuePlace { get; set; }
            [XmlElement(ElementName = "IssueDate")]
            public string IssueDate { get; set; }
            [XmlElement(ElementName = "ResponseXML")]
            public string ResponseXML { get; set; }
            [XmlElement(ElementName = "Request")]
            public string Request { get; set; }
            [XmlElement(ElementName = "Response")]
            public Response Response { get; set; }
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
            [XmlElement(ElementName = "RequestCount")]
            public string RequestCount { get; set; }
            [XmlElement(ElementName = "CustomKey2")]
            public string CustomKey2 { get; set; }
            [XmlElement(ElementName = "CustomKey1")]
            public string CustomKey1 { get; set; }
            [XmlElement(ElementName = "Key4")]
            public string Key4 { get; set; }
            [XmlElement(ElementName = "Key3")]
            public string Key3 { get; set; }
            [XmlElement(ElementName = "Key2")]
            public string Key2 { get; set; }
            [XmlElement(ElementName = "Key1")]
            public string Key1 { get; set; }
            [XmlElement(ElementName = "CheckOutput")]
            public string CheckOutput { get; set; }
            [XmlElement(ElementName = "RiskLevel")]
            public string RiskLevel { get; set; }
            [XmlElement(ElementName = "VelocityCheckReasons")]
            public VelocityCheckReasons VelocityCheckReasons { get; set; }
        }

        [XmlRoot(ElementName = "VelocityCheckStatus")]
        public class VelocityCheckStatus
        {
            [XmlElement(ElementName = "Outcome")]
            public string Outcome { get; set; }
            [XmlElement(ElementName = "Current")]
            public string Current { get; set; }
            [XmlElement(ElementName = "IsSuccess")]
            public string IsSuccess { get; set; }
        }

        [XmlRoot(ElementName = "CriteriaPostBureauCOL")]
        public class CriteriaPostBureauCOL
        {
            [XmlElement(ElementName = "PostBureauReasons")]
            public PostBureauReasons PostBureauReasons { get; set; }
        }

        [XmlRoot(ElementName = "ExpertoAttributes2059")]
        public class ExpertoAttributes2059
        {
            [XmlElement(ElementName = "TO_080_r")]
            public string TO_080_r { get; set; }
            [XmlElement(ElementName = "TO_079_r")]
            public string TO_079_r { get; set; }
            [XmlElement(ElementName = "PuntajeClear")]
            public string PuntajeClear { get; set; }
            [XmlElement(ElementName = "puntaje_tsi_TDC")]
            public string Puntaje_tsi_TDC { get; set; }
            [XmlElement(ElementName = "puntaje_tsi_con")]
            public string Puntaje_tsi_con { get; set; }
            [XmlElement(ElementName = "PuntajePYME_C2")]
            public string PuntajePYME_C2 { get; set; }
            [XmlElement(ElementName = "SR_221")]
            public string SR_221 { get; set; }
            [XmlElement(ElementName = "CT_037")]
            public string CT_037 { get; set; }
            [XmlElement(ElementName = "CT_042")]
            public string CT_042 { get; set; }
            [XmlElement(ElementName = "CT_055")]
            public string CT_055 { get; set; }
            [XmlElement(ElementName = "CT_131")]
            public string CT_131 { get; set; }
            [XmlElement(ElementName = "CT_136")]
            public string CT_136 { get; set; }
            [XmlElement(ElementName = "CT_137")]
            public string CT_137 { get; set; }
            [XmlElement(ElementName = "CT_383")]
            public string CT_383 { get; set; }
            [XmlElement(ElementName = "CT_409")]
            public string CT_409 { get; set; }
            [XmlElement(ElementName = "CT_410")]
            public string CT_410 { get; set; }
            [XmlElement(ElementName = "CT_246")]
            public string CT_246 { get; set; }
            [XmlElement(ElementName = "CT_261")]
            public string CT_261 { get; set; }
            [XmlElement(ElementName = "CT_262")]
            public string CT_262 { get; set; }
            [XmlElement(ElementName = "SS_024")]
            public string SS_024 { get; set; }
            [XmlElement(ElementName = "CT_024")]
            public string CT_024 { get; set; }
            [XmlElement(ElementName = "SR_135")]
            public string SR_135 { get; set; }
            [XmlElement(ElementName = "SR_136")]
            public string SR_136 { get; set; }
            [XmlElement(ElementName = "SR_139")]
            public string SR_139 { get; set; }
            [XmlElement(ElementName = "SR_143")]
            public string SR_143 { get; set; }
            [XmlElement(ElementName = "SS_206")]
            public string SS_206 { get; set; }
            [XmlElement(ElementName = "TC_013")]
            public string TC_013 { get; set; }
            [XmlElement(ElementName = "TC_014")]
            public string TC_014 { get; set; }
            [XmlElement(ElementName = "TC_016")]
            public string TC_016 { get; set; }
            [XmlElement(ElementName = "TC_161")]
            public string TC_161 { get; set; }
            [XmlElement(ElementName = "TC_166")]
            public string TC_166 { get; set; }
            [XmlElement(ElementName = "TC_171")]
            public string TC_171 { get; set; }
            [XmlElement(ElementName = "TC_175")]
            public string TC_175 { get; set; }
            [XmlElement(ElementName = "TC_176")]
            public string TC_176 { get; set; }
            [XmlElement(ElementName = "TC_206")]
            public string TC_206 { get; set; }
            [XmlElement(ElementName = "SS_223")]
            public string SS_223 { get; set; }
            [XmlElement(ElementName = "SS_261")]
            public string SS_261 { get; set; }
            [XmlElement(ElementName = "SR_104")]
            public string SR_104 { get; set; }
            [XmlElement(ElementName = "SR_105")]
            public string SR_105 { get; set; }
            [XmlElement(ElementName = "SR_110")]
            public string SR_110 { get; set; }
            [XmlElement(ElementName = "SS_207")]
            public string SS_207 { get; set; }
            [XmlElement(ElementName = "TC_172")]
            public string TC_172 { get; set; }
            [XmlElement(ElementName = "CA_003")]
            public string CA_003 { get; set; }
            [XmlElement(ElementName = "CT_004")]
            public string CT_004 { get; set; }
            [XmlElement(ElementName = "CT_019")]
            public string CT_019 { get; set; }
            [XmlElement(ElementName = "CT_020")]
            public string CT_020 { get; set; }
            [XmlElement(ElementName = "SF_053")]
            public string SF_053 { get; set; }
            [XmlElement(ElementName = "SR_187")]
            public string SR_187 { get; set; }
            [XmlElement(ElementName = "SS_268")]
            public string SS_268 { get; set; }
            [XmlElement(ElementName = "SS_282")]
            public string SS_282 { get; set; }
            [XmlElement(ElementName = "CC_104")]
            public string CC_104 { get; set; }
            [XmlElement(ElementName = "CC_105")]
            public string CC_105 { get; set; }
            [XmlElement(ElementName = "SR_128")]
            public string SR_128 { get; set; }
            [XmlElement(ElementName = "SS_221")]
            public string SS_221 { get; set; }
            [XmlElement(ElementName = "SS_289")]
            public string SS_289 { get; set; }
            [XmlElement(ElementName = "SS_278")]
            public string SS_278 { get; set; }
            [XmlElement(ElementName = "CT_040")]
            public string CT_040 { get; set; }
            [XmlElement(ElementName = "CO_066")]
            public string CO_066 { get; set; }
            [XmlElement(ElementName = "CO_068")]
            public string CO_068 { get; set; }
            [XmlElement(ElementName = "CO_067")]
            public string CO_067 { get; set; }
            [XmlElement(ElementName = "TC_018")]
            public string TC_018 { get; set; }
            [XmlElement(ElementName = "SF_066")]
            public string SF_066 { get; set; }
            [XmlElement(ElementName = "CO_016")]
            public string CO_016 { get; set; }
            [XmlElement(ElementName = "CT_192")]
            public string CT_192 { get; set; }
            [XmlElement(ElementName = "CT_193")]
            public string CT_193 { get; set; }
            [XmlElement(ElementName = "CT_194")]
            public string CT_194 { get; set; }
            [XmlElement(ElementName = "CT_195")]
            public string CT_195 { get; set; }
            [XmlElement(ElementName = "TC_157")]
            public string TC_157 { get; set; }
            [XmlElement(ElementName = "TC_158")]
            public string TC_158 { get; set; }
            [XmlElement(ElementName = "TC_159")]
            public string TC_159 { get; set; }
            [XmlElement(ElementName = "TC_160")]
            public string TC_160 { get; set; }
            [XmlElement(ElementName = "SS_210")]
            public string SS_210 { get; set; }
            [XmlElement(ElementName = "SS_211")]
            public string SS_211 { get; set; }
            [XmlElement(ElementName = "SS_214")]
            public string SS_214 { get; set; }
            [XmlElement(ElementName = "SR_114")]
            public string SR_114 { get; set; }
            [XmlElement(ElementName = "SS_216")]
            public string SS_216 { get; set; }
            [XmlElement(ElementName = "SS_218")]
            public string SS_218 { get; set; }
            [XmlElement(ElementName = "SR_118")]
            public string SR_118 { get; set; }
            [XmlElement(ElementName = "SS_219")]
            public string SS_219 { get; set; }
            [XmlElement(ElementName = "SR_119")]
            public string SR_119 { get; set; }
            [XmlElement(ElementName = "SS_220")]
            public string SS_220 { get; set; }
            [XmlElement(ElementName = "SR_120")]
            public string SR_120 { get; set; }
            [XmlElement(ElementName = "SR_121")]
            public string SR_121 { get; set; }
            [XmlElement(ElementName = "CO_017")]
            public string CO_017 { get; set; }
            [XmlElement(ElementName = "CO_024")]
            public string CO_024 { get; set; }
            [XmlElement(ElementName = "CO_035")]
            public string CO_035 { get; set; }
            [XmlElement(ElementName = "TO_031")]
            public string TO_031 { get; set; }
            [XmlElement(ElementName = "TO_032")]
            public string TO_032 { get; set; }
            [XmlElement(ElementName = "TO_034")]
            public string TO_034 { get; set; }
            [XmlElement(ElementName = "TO_054")]
            public string TO_054 { get; set; }
            [XmlElement(ElementName = "TO_084")]
            public string TO_084 { get; set; }
            [XmlElement(ElementName = "SF_028")]
            public string SF_028 { get; set; }
            [XmlElement(ElementName = "TO_020")]
            public string TO_020 { get; set; }
            [XmlElement(ElementName = "TO_051")]
            public string TO_051 { get; set; }
            [XmlElement(ElementName = "TO_016")]
            public string TO_016 { get; set; }
            [XmlElement(ElementName = "TO_013")]
            public string TO_013 { get; set; }
            [XmlElement(ElementName = "TO_048")]
            public string TO_048 { get; set; }
            [XmlElement(ElementName = "TO_061")]
            public string TO_061 { get; set; }
            [XmlElement(ElementName = "TO_083")]
            public string TO_083 { get; set; }
            [XmlElement(ElementName = "TO_100")]
            public string TO_100 { get; set; }
            [XmlElement(ElementName = "TO_086")]
            public string TO_086 { get; set; }
            [XmlElement(ElementName = "SF_065")]
            public string SF_065 { get; set; }
            [XmlElement(ElementName = "SF_070")]
            public string SF_070 { get; set; }
            [XmlElement(ElementName = "SF_082")]
            public string SF_082 { get; set; }
            [XmlElement(ElementName = "SF_044")]
            public string SF_044 { get; set; }
            [XmlElement(ElementName = "TO_009")]
            public string TO_009 { get; set; }
            [XmlElement(ElementName = "TO_010")]
            public string TO_010 { get; set; }
            [XmlElement(ElementName = "SR_103")]
            public string SR_103 { get; set; }
            [XmlElement(ElementName = "TO_082")]
            public string TO_082 { get; set; }
            [XmlElement(ElementName = "TO_080_R")]
            public string TO_080_R { get; set; }
            [XmlElement(ElementName = "SS_291")]
            public string SS_291 { get; set; }
            [XmlElement(ElementName = "SF_025")]
            public string SF_025 { get; set; }
            [XmlElement(ElementName = "CT_028")]
            public string CT_028 { get; set; }
            [XmlElement(ElementName = "SR_123")]
            public string SR_123 { get; set; }
            [XmlElement(ElementName = "SR_220")]
            public string SR_220 { get; set; }
            [XmlElement(ElementName = "CT_027")]
            public string CT_027 { get; set; }
            [XmlElement(ElementName = "CT_502")]
            public string CT_502 { get; set; }
            [XmlElement(ElementName = "TO_085")]
            public string TO_085 { get; set; }
            [XmlElement(ElementName = "TO_087")]
            public string TO_087 { get; set; }
            [XmlElement(ElementName = "CON_008")]
            public string CON_008 { get; set; }
            [XmlElement(ElementName = "TC_221")]
            public string TC_221 { get; set; }
            [XmlElement(ElementName = "CO_086")]
            public string CO_086 { get; set; }
            [XmlElement(ElementName = "CT_820")]
            public string CT_820 { get; set; }
            [XmlElement(ElementName = "CT_837")]
            public string CT_837 { get; set; }
            [XmlElement(ElementName = "SR_004")]
            public string SR_004 { get; set; }
            [XmlElement(ElementName = "SR_008")]
            public string SR_008 { get; set; }
            [XmlElement(ElementName = "CT_168")]
            public string CT_168 { get; set; }
            [XmlElement(ElementName = "CT_169")]
            public string CT_169 { get; set; }
            [XmlElement(ElementName = "CT_170")]
            public string CT_170 { get; set; }
            [XmlElement(ElementName = "CT_171")]
            public string CT_171 { get; set; }
            [XmlElement(ElementName = "CT_172")]
            public string CT_172 { get; set; }
            [XmlElement(ElementName = "CT_173")]
            public string CT_173 { get; set; }
            [XmlElement(ElementName = "CT_174")]
            public string CT_174 { get; set; }
            [XmlElement(ElementName = "CT_175")]
            public string CT_175 { get; set; }
            [XmlElement(ElementName = "CT_176")]
            public string CT_176 { get; set; }
            [XmlElement(ElementName = "CT_177")]
            public string CT_177 { get; set; }
            [XmlElement(ElementName = "CT_178")]
            public string CT_178 { get; set; }
            [XmlElement(ElementName = "CT_179")]
            public string CT_179 { get; set; }
            [XmlElement(ElementName = "CT_212")]
            public string CT_212 { get; set; }
            [XmlElement(ElementName = "CT_375")]
            public string CT_375 { get; set; }
            [XmlElement(ElementName = "CT_213")]
            public string CT_213 { get; set; }
            [XmlElement(ElementName = "CT_376")]
            public string CT_376 { get; set; }
            [XmlElement(ElementName = "CT_214")]
            public string CT_214 { get; set; }
            [XmlElement(ElementName = "CT_377")]
            public string CT_377 { get; set; }
            [XmlElement(ElementName = "CT_215")]
            public string CT_215 { get; set; }
            [XmlElement(ElementName = "CT_378")]
            public string CT_378 { get; set; }
            [XmlElement(ElementName = "TC_151")]
            public string TC_151 { get; set; }
            [XmlElement(ElementName = "TC_152")]
            public string TC_152 { get; set; }
            [XmlElement(ElementName = "TC_153")]
            public string TC_153 { get; set; }
            [XmlElement(ElementName = "TO_113")]
            public string TO_113 { get; set; }
            [XmlElement(ElementName = "TO_059")]
            public string TO_059 { get; set; }
            [XmlElement(ElementName = "TO_046")]
            public string TO_046 { get; set; }
            [XmlElement(ElementName = "CT_196")]
            public string CT_196 { get; set; }
            [XmlElement(ElementName = "CT_197")]
            public string CT_197 { get; set; }
            [XmlElement(ElementName = "CT_198")]
            public string CT_198 { get; set; }
            [XmlElement(ElementName = "CT_199")]
            public string CT_199 { get; set; }
            [XmlElement(ElementName = "CT_821")]
            public string CT_821 { get; set; }
            [XmlElement(ElementName = "CT_200")]
            public string CT_200 { get; set; }
            [XmlElement(ElementName = "CT_201")]
            public string CT_201 { get; set; }
            [XmlElement(ElementName = "CT_202")]
            public string CT_202 { get; set; }
            [XmlElement(ElementName = "CT_203")]
            public string CT_203 { get; set; }
            [XmlElement(ElementName = "CT_822")]
            public string CT_822 { get; set; }
            [XmlElement(ElementName = "CT_204")]
            public string CT_204 { get; set; }
            [XmlElement(ElementName = "CT_205")]
            public string CT_205 { get; set; }
            [XmlElement(ElementName = "CT_206")]
            public string CT_206 { get; set; }
            [XmlElement(ElementName = "CT_207")]
            public string CT_207 { get; set; }
            [XmlElement(ElementName = "TO_081_R")]
            public string TO_081_R { get; set; }
            [XmlElement(ElementName = "TO_081")]
            public string TO_081 { get; set; }
            [XmlElement(ElementName = "SF_058")]
            public string SF_058 { get; set; }
            [XmlElement(ElementName = "SR_243")]
            public string SR_243 { get; set; }
            [XmlElement(ElementName = "SR_133")]
            public string SR_133 { get; set; }
            [XmlElement(ElementName = "SR_134")]
            public string SR_134 { get; set; }
            [XmlElement(ElementName = "SF_010")]
            public string SF_010 { get; set; }
            [XmlElement(ElementName = "CT_025")]
            public string CT_025 { get; set; }
            [XmlElement(ElementName = "CT_018")]
            public string CT_018 { get; set; }
            [XmlElement(ElementName = "TC_001")]
            public string TC_001 { get; set; }
            [XmlElement(ElementName = "SF_002")]
            public string SF_002 { get; set; }
            [XmlElement(ElementName = "SF_003")]
            public string SF_003 { get; set; }
            [XmlElement(ElementName = "SS_021")]
            public string SS_021 { get; set; }
            [XmlElement(ElementName = "SS_022")]
            public string SS_022 { get; set; }
            [XmlElement(ElementName = "SR_001")]
            public string SR_001 { get; set; }
            [XmlElement(ElementName = "SR_002")]
            public string SR_002 { get; set; }
            [XmlElement(ElementName = "SR_005")]
            public string SR_005 { get; set; }
            [XmlElement(ElementName = "SR_006")]
            public string SR_006 { get; set; }
            [XmlElement(ElementName = "TO_001")]
            public string TO_001 { get; set; }
            [XmlElement(ElementName = "TO_002")]
            public string TO_002 { get; set; }
            [XmlElement(ElementName = "SR_111")]
            public string SR_111 { get; set; }
            [XmlElement(ElementName = "TC_178")]
            public string TC_178 { get; set; }
            [XmlElement(ElementName = "SS_231")]
            public string SS_231 { get; set; }
            [XmlElement(ElementName = "CT_387")]
            public string CT_387 { get; set; }
            [XmlElement(ElementName = "CON_002")]
            public string CON_002 { get; set; }
            [XmlElement(ElementName = "CON_003")]
            public string CON_003 { get; set; }
            [XmlElement(ElementName = "CON_004")]
            public string CON_004 { get; set; }
            [XmlElement(ElementName = "CON_005")]
            public string CON_005 { get; set; }
            [XmlElement(ElementName = "CON_007")]
            public string CON_007 { get; set; }
            [XmlElement(ElementName = "TO_011")]
            public string TO_011 { get; set; }
            [XmlElement(ElementName = "TO_030")]
            public string TO_030 { get; set; }
            [XmlElement(ElementName = "TO_008")]
            public string TO_008 { get; set; }
            [XmlElement(ElementName = "CT_814")]
            public string CT_814 { get; set; }
            [XmlElement(ElementName = "CT_815")]
            public string CT_815 { get; set; }
            [XmlElement(ElementName = "CT_816")]
            public string CT_816 { get; set; }
            [XmlElement(ElementName = "TO_044")]
            public string TO_044 { get; set; }
            [XmlElement(ElementName = "TO_043")]
            public string TO_043 { get; set; }
            [XmlElement(ElementName = "TO_041")]
            public string TO_041 { get; set; }
            [XmlElement(ElementName = "TO_037")]
            public string TO_037 { get; set; }
            [XmlElement(ElementName = "SF_036")]
            public string SF_036 { get; set; }
            [XmlElement(ElementName = "SF_035")]
            public string SF_035 { get; set; }
            [XmlElement(ElementName = "SF_030")]
            public string SF_030 { get; set; }
            [XmlElement(ElementName = "SF_029")]
            public string SF_029 { get; set; }
            [XmlElement(ElementName = "SF_038")]
            public string SF_038 { get; set; }
            [XmlElement(ElementName = "CON_001")]
            public string CON_001 { get; set; }
            [XmlElement(ElementName = "CON_010")]
            public string CON_010 { get; set; }
            [XmlElement(ElementName = "CON_011")]
            public string CON_011 { get; set; }
            [XmlElement(ElementName = "TO_012")]
            public string TO_012 { get; set; }
            [XmlElement(ElementName = "TO_042")]
            public string TO_042 { get; set; }
            [XmlElement(ElementName = "END_001")]
            public string END_001 { get; set; }
            [XmlElement(ElementName = "SF_051")]
            public string SF_051 { get; set; }
            [XmlElement(ElementName = "SF_069")]
            public string SF_069 { get; set; }
            [XmlElement(ElementName = "TO_049")]
            public string TO_049 { get; set; }
            [XmlElement(ElementName = "TO_060")]
            public string TO_060 { get; set; }
            [XmlElement(ElementName = "TO_069")]
            public string TO_069 { get; set; }
            [XmlElement(ElementName = "TO_079_R")]
            public string TO_079_R { get; set; }
            [XmlElement(ElementName = "CT_054")]
            public string CT_054 { get; set; }
            [XmlElement(ElementName = "FlowType")]
            public string FlowType { get; set; }
            [XmlElement(ElementName = "Nombre")]
            public string Nombre { get; set; }
            [XmlElement(ElementName = "RANGOEDAD")]
            public string RANGOEDAD { get; set; }
            [XmlElement(ElementName = "GENERO")]
            public string GENERO { get; set; }
            [XmlElement(ElementName = "GENERO_CONS")]
            public string GENERO_CONS { get; set; }
            [XmlElement(ElementName = "CANCELACION")]
            public string CANCELACION { get; set; }
            [XmlElement(ElementName = "FECHA_EXPEDICION_ID")]
            public string FECHA_EXPEDICION_ID { get; set; }
            [XmlElement(ElementName = "LUGAR_EXPEDICION")]
            public string LUGAR_EXPEDICION { get; set; }
        }

        [XmlRoot(ElementName = "DSUbicaCOL")]
        public class DSUbicaCOL
        {
            [XmlElement(ElementName = "RiskLevel")]
            public string RiskLevel { get; set; }
            [XmlElement(ElementName = "IDVOutput")]
            public string IDVOutput { get; set; }
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
            [XmlElement(ElementName = "CriteriaPostBureauCOL")]
            public CriteriaPostBureauCOL CriteriaPostBureauCOL { get; set; }
            [XmlElement(ElementName = "ExpertoAttributes2059")]
            public ExpertoAttributes2059 ExpertoAttributes2059 { get; set; }
            [XmlElement(ElementName = "DSUbicaCOL")]
            public DSUbicaCOL DSUbicaCOL { get; set; }
            [XmlElement(ElementName = "Derived")]
            public Derived Derived { get; set; }
        }

        [XmlRoot(ElementName = "QueueRequest")]
        public class QueueRequest
        {
            [XmlElement(ElementName = "GenerateReport")]
            public string GenerateReport { get; set; }
            [XmlElement(ElementName = "CommentText")]
            public string CommentText { get; set; }
        }

        [XmlRoot(ElementName = "Consulta")]
        public class Consulta
        {
            [XmlElement(ElementName = "CodigoEntidad")]
            public string CodigoEntidad { get; set; }
            [XmlElement(ElementName = "CodigoTipoEntidad")]
            public string CodigoTipoEntidad { get; set; }
            [XmlElement(ElementName = "MotivoConsulta")]
            public string MotivoConsulta { get; set; }
            [XmlElement(ElementName = "Ciudad")]
            public string Ciudad { get; set; }
            [XmlElement(ElementName = "Sucursal")]
            public string Sucursal { get; set; }
            [XmlElement(ElementName = "NombreEntidad")]
            public string NombreEntidad { get; set; }
            [XmlElement(ElementName = "NombreTipoEntidad")]
            public string NombreTipoEntidad { get; set; }
            [XmlElement(ElementName = "FechaConsulta")]
            public string FechaConsulta { get; set; }
        }

        [XmlRoot(ElementName = "HuellaConsulta")]
        public class HuellaConsulta
        {
            [XmlElement(ElementName = "Consulta")]
            public List<Consulta> Consulta { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento93")]
        public class Endeudamiento93
        {
            [XmlElement(ElementName = "CumplimientoCuota")]
            public string CumplimientoCuota { get; set; }
            [XmlElement(ElementName = "CuotaEsperada")]
            public string CuotaEsperada { get; set; }
            [XmlElement(ElementName = "FechaUltimoAvaluo")]
            public string FechaUltimoAvaluo { get; set; }
            [XmlElement(ElementName = "TipoGarantia")]
            public string TipoGarantia { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantia")]
            public string CubrimientoGarantia { get; set; }
            [XmlElement(ElementName = "ParticipacionTotalDeudas")]
            public string ParticipacionTotalDeudas { get; set; }
            [XmlElement(ElementName = "ValorDeuda")]
            public string ValorDeuda { get; set; }
            [XmlElement(ElementName = "NumeroOperadores")]
            public string NumeroOperadores { get; set; }
            [XmlElement(ElementName = "TipoMoneda")]
            public string TipoMoneda { get; set; }
            [XmlElement(ElementName = "Calificacion")]
            public string Calificacion { get; set; }
            [XmlElement(ElementName = "ModalidadCredito")]
            public string ModalidadCredito { get; set; }
            [XmlElement(ElementName = "NumeroFideicomiso")]
            public string NumeroFideicomiso { get; set; }
            [XmlElement(ElementName = "TipoFideicomiso")]
            public string TipoFideicomiso { get; set; }
            [XmlElement(ElementName = "EntidadOriginadoraCartera")]
            public string EntidadOriginadoraCartera { get; set; }
            [XmlElement(ElementName = "TipoEntidadOriginadoraCartera")]
            public string TipoEntidadOriginadoraCartera { get; set; }
            [XmlElement(ElementName = "NombreEntidad")]
            public string NombreEntidad { get; set; }
            [XmlElement(ElementName = "TipoEntidad")]
            public string TipoEntidad { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento92")]
        public class Endeudamiento92
        {
            [XmlElement(ElementName = "CumplimientoCuota")]
            public string CumplimientoCuota { get; set; }
            [XmlElement(ElementName = "CuotaEsperada")]
            public string CuotaEsperada { get; set; }
            [XmlElement(ElementName = "ValorContingencias")]
            public string ValorContingencias { get; set; }
            [XmlElement(ElementName = "NumeroContingencias")]
            public string NumeroContingencias { get; set; }
            [XmlElement(ElementName = "TipoMoneda")]
            public string TipoMoneda { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento91")]
        public class Endeudamiento91
        {
            [XmlElement(ElementName = "CubrimientoGarantiaMicrocredito")]
            public string CubrimientoGarantiaMicrocredito { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaVivienda")]
            public string CubrimientoGarantiaVivienda { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaConsumo")]
            public string CubrimientoGarantiaConsumo { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaComercial")]
            public string CubrimientoGarantiaComercial { get; set; }
            [XmlElement(ElementName = "ParticipacionTotalDeudas")]
            public string ParticipacionTotalDeudas { get; set; }
            [XmlElement(ElementName = "Total")]
            public string Total { get; set; }
            [XmlElement(ElementName = "ValorDeudaMicrocredito")]
            public string ValorDeudaMicrocredito { get; set; }
            [XmlElement(ElementName = "ValorDeudaVivienda")]
            public string ValorDeudaVivienda { get; set; }
            [XmlElement(ElementName = "ValorDeudaConsumo")]
            public string ValorDeudaConsumo { get; set; }
            [XmlElement(ElementName = "ValorDeudaComercial")]
            public string ValorDeudaComercial { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesMicrocredito")]
            public string NumeroOperacionesMicrocredito { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesVivienda")]
            public string NumeroOperacionesVivienda { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesConsumo")]
            public string NumeroOperacionesConsumo { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesComercial")]
            public string NumeroOperacionesComercial { get; set; }
            [XmlElement(ElementName = "TipoModena")]
            public string TipoModena { get; set; }
            [XmlElement(ElementName = "Calificacion")]
            public string Calificacion { get; set; }
        }

        [XmlRoot(ElementName = "EndeudamientoTrimIII")]
        public class EndeudamientoTrimIII
        {
            [XmlElement(ElementName = "Endeudamiento93")]
            public List<Endeudamiento93> Endeudamiento93 { get; set; }
            [XmlElement(ElementName = "Endeudamiento92")]
            public List<Endeudamiento92> Endeudamiento92 { get; set; }
            [XmlElement(ElementName = "Endeudamiento91")]
            public List<Endeudamiento91> Endeudamiento91 { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento83")]
        public class Endeudamiento83
        {
            [XmlElement(ElementName = "CumplimientoCuota")]
            public string CumplimientoCuota { get; set; }
            [XmlElement(ElementName = "CuotaEsperada")]
            public string CuotaEsperada { get; set; }
            [XmlElement(ElementName = "FechaUltimoAvaluo")]
            public string FechaUltimoAvaluo { get; set; }
            [XmlElement(ElementName = "TipoGarantia")]
            public string TipoGarantia { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantia")]
            public string CubrimientoGarantia { get; set; }
            [XmlElement(ElementName = "ParticipacionTotalDeudas")]
            public string ParticipacionTotalDeudas { get; set; }
            [XmlElement(ElementName = "ValorDeuda")]
            public string ValorDeuda { get; set; }
            [XmlElement(ElementName = "NumeroOperadores")]
            public string NumeroOperadores { get; set; }
            [XmlElement(ElementName = "TipoMoneda")]
            public string TipoMoneda { get; set; }
            [XmlElement(ElementName = "Calificacion")]
            public string Calificacion { get; set; }
            [XmlElement(ElementName = "ModalidadCredito")]
            public string ModalidadCredito { get; set; }
            [XmlElement(ElementName = "NumeroFideicomiso")]
            public string NumeroFideicomiso { get; set; }
            [XmlElement(ElementName = "TipoFideicomiso")]
            public string TipoFideicomiso { get; set; }
            [XmlElement(ElementName = "EntidadOriginadoraCartera")]
            public string EntidadOriginadoraCartera { get; set; }
            [XmlElement(ElementName = "TipoEntidadOriginadoraCartera")]
            public string TipoEntidadOriginadoraCartera { get; set; }
            [XmlElement(ElementName = "NombreEntidad")]
            public string NombreEntidad { get; set; }
            [XmlElement(ElementName = "TipoEntidad")]
            public string TipoEntidad { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento82")]
        public class Endeudamiento82
        {
            [XmlElement(ElementName = "CumplimientoCuota")]
            public string CumplimientoCuota { get; set; }
            [XmlElement(ElementName = "CuotaEsperada")]
            public string CuotaEsperada { get; set; }
            [XmlElement(ElementName = "ValorContingencias")]
            public string ValorContingencias { get; set; }
            [XmlElement(ElementName = "NumeroContingencias")]
            public string NumeroContingencias { get; set; }
            [XmlElement(ElementName = "TipoMoneda")]
            public string TipoMoneda { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento81")]
        public class Endeudamiento81
        {
            [XmlElement(ElementName = "CubrimientoGarantiaMicrocredito")]
            public string CubrimientoGarantiaMicrocredito { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaVivienda")]
            public string CubrimientoGarantiaVivienda { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaConsumo")]
            public string CubrimientoGarantiaConsumo { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaComercial")]
            public string CubrimientoGarantiaComercial { get; set; }
            [XmlElement(ElementName = "ParticipacionTotalDeudas")]
            public string ParticipacionTotalDeudas { get; set; }
            [XmlElement(ElementName = "Total")]
            public string Total { get; set; }
            [XmlElement(ElementName = "ValorDeudaMicrocredito")]
            public string ValorDeudaMicrocredito { get; set; }
            [XmlElement(ElementName = "ValorDeudaVivienda")]
            public string ValorDeudaVivienda { get; set; }
            [XmlElement(ElementName = "ValorDeudaConsumo")]
            public string ValorDeudaConsumo { get; set; }
            [XmlElement(ElementName = "ValorDeudaComercial")]
            public string ValorDeudaComercial { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesMicrocredito")]
            public string NumeroOperacionesMicrocredito { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesVivienda")]
            public string NumeroOperacionesVivienda { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesConsumo")]
            public string NumeroOperacionesConsumo { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesComercial")]
            public string NumeroOperacionesComercial { get; set; }
            [XmlElement(ElementName = "TipoModena")]
            public string TipoModena { get; set; }
            [XmlElement(ElementName = "Calificacion")]
            public string Calificacion { get; set; }
        }

        [XmlRoot(ElementName = "EndeudamientoTrimII")]
        public class EndeudamientoTrimII
        {
            [XmlElement(ElementName = "Endeudamiento83")]
            public List<Endeudamiento83> Endeudamiento83 { get; set; }
            [XmlElement(ElementName = "Endeudamiento82")]
            public List<Endeudamiento82> Endeudamiento82 { get; set; }
            [XmlElement(ElementName = "Endeudamiento81")]
            public List<Endeudamiento81> Endeudamiento81 { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento73")]
        public class Endeudamiento73
        {
            [XmlElement(ElementName = "CumplimientoCuota")]
            public string CumplimientoCuota { get; set; }
            [XmlElement(ElementName = "CuotaEsperada")]
            public string CuotaEsperada { get; set; }
            [XmlElement(ElementName = "FechaUltimoAvaluo")]
            public string FechaUltimoAvaluo { get; set; }
            [XmlElement(ElementName = "TipoGarantia")]
            public string TipoGarantia { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantia")]
            public string CubrimientoGarantia { get; set; }
            [XmlElement(ElementName = "ParticipacionTotalDeudas")]
            public string ParticipacionTotalDeudas { get; set; }
            [XmlElement(ElementName = "ValorDeuda")]
            public string ValorDeuda { get; set; }
            [XmlElement(ElementName = "NumeroOperadores")]
            public string NumeroOperadores { get; set; }
            [XmlElement(ElementName = "TipoMoneda")]
            public string TipoMoneda { get; set; }
            [XmlElement(ElementName = "Calificacion")]
            public string Calificacion { get; set; }
            [XmlElement(ElementName = "ModalidadCredito")]
            public string ModalidadCredito { get; set; }
            [XmlElement(ElementName = "NumeroFideicomiso")]
            public string NumeroFideicomiso { get; set; }
            [XmlElement(ElementName = "TipoFideicomiso")]
            public string TipoFideicomiso { get; set; }
            [XmlElement(ElementName = "EntidadOriginadoraCartera")]
            public string EntidadOriginadoraCartera { get; set; }
            [XmlElement(ElementName = "TipoEntidadOriginadoraCartera")]
            public string TipoEntidadOriginadoraCartera { get; set; }
            [XmlElement(ElementName = "NombreEntidad")]
            public string NombreEntidad { get; set; }
            [XmlElement(ElementName = "TipoEntidad")]
            public string TipoEntidad { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento72")]
        public class Endeudamiento72
        {
            [XmlElement(ElementName = "CumplimientoCuota")]
            public string CumplimientoCuota { get; set; }
            [XmlElement(ElementName = "CuotaEsperada")]
            public string CuotaEsperada { get; set; }
            [XmlElement(ElementName = "ValorContingencias")]
            public string ValorContingencias { get; set; }
            [XmlElement(ElementName = "NumeroContingencias")]
            public string NumeroContingencias { get; set; }
            [XmlElement(ElementName = "TipoMoneda")]
            public string TipoMoneda { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento71")]
        public class Endeudamiento71
        {
            [XmlElement(ElementName = "CubrimientoGarantiaMicrocredito")]
            public string CubrimientoGarantiaMicrocredito { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaVivienda")]
            public string CubrimientoGarantiaVivienda { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaConsumo")]
            public string CubrimientoGarantiaConsumo { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantiaComercial")]
            public string CubrimientoGarantiaComercial { get; set; }
            [XmlElement(ElementName = "ParticipacionTotalDeudas")]
            public string ParticipacionTotalDeudas { get; set; }
            [XmlElement(ElementName = "Total")]
            public string Total { get; set; }
            [XmlElement(ElementName = "ValorDeudaMicrocredito")]
            public string ValorDeudaMicrocredito { get; set; }
            [XmlElement(ElementName = "ValorDeudaVivienda")]
            public string ValorDeudaVivienda { get; set; }
            [XmlElement(ElementName = "ValorDeudaConsumo")]
            public string ValorDeudaConsumo { get; set; }
            [XmlElement(ElementName = "ValorDeudaComercial")]
            public string ValorDeudaComercial { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesMicrocredito")]
            public string NumeroOperacionesMicrocredito { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesVivienda")]
            public string NumeroOperacionesVivienda { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesConsumo")]
            public string NumeroOperacionesConsumo { get; set; }
            [XmlElement(ElementName = "NumeroOperacionesComercial")]
            public string NumeroOperacionesComercial { get; set; }
            [XmlElement(ElementName = "TipoModena")]
            public string TipoModena { get; set; }
            [XmlElement(ElementName = "Calificacion")]
            public string Calificacion { get; set; }
        }

        [XmlRoot(ElementName = "EndeudamientoTrimI")]
        public class EndeudamientoTrimI
        {
            [XmlElement(ElementName = "Endeudamiento73")]
            public List<Endeudamiento73> Endeudamiento73 { get; set; }
            [XmlElement(ElementName = "Endeudamiento72")]
            public List<Endeudamiento72> Endeudamiento72 { get; set; }
            [XmlElement(ElementName = "Endeudamiento71")]
            public List<Endeudamiento71> Endeudamiento71 { get; set; }
        }

        [XmlRoot(ElementName = "EncabezadoEndeudamiento")]
        public class EncabezadoEndeudamiento
        {
            [XmlElement(ElementName = "NumeroCastigosTrimestreIII")]
            public string NumeroCastigosTrimestreIII { get; set; }
            [XmlElement(ElementName = "NumeroCastigosTrimestreII")]
            public string NumeroCastigosTrimestreII { get; set; }
            [XmlElement(ElementName = "NumeroCastigosTrimestreI")]
            public string NumeroCastigosTrimestreI { get; set; }
            [XmlElement(ElementName = "NumeroReestructuracionesTrimestreIII")]
            public string NumeroReestructuracionesTrimestreIII { get; set; }
            [XmlElement(ElementName = "NumeroReestructuracionesTrimestreII")]
            public string NumeroReestructuracionesTrimestreII { get; set; }
            [XmlElement(ElementName = "NumeroReestructuracionesTrimestreI")]
            public string NumeroReestructuracionesTrimestreI { get; set; }
            [XmlElement(ElementName = "NumeroComprasTrimestreIII")]
            public string NumeroComprasTrimestreIII { get; set; }
            [XmlElement(ElementName = "NumeroComprasTrimestreII")]
            public string NumeroComprasTrimestreII { get; set; }
            [XmlElement(ElementName = "NumeroComprasTrimestreI")]
            public string NumeroComprasTrimestreI { get; set; }
            [XmlElement(ElementName = "FechaTrimestreIII")]
            public string FechaTrimestreIII { get; set; }
            [XmlElement(ElementName = "NumeroEntidadesTrimestreIII")]
            public string NumeroEntidadesTrimestreIII { get; set; }
            [XmlElement(ElementName = "FechaTrimestreII")]
            public string FechaTrimestreII { get; set; }
            [XmlElement(ElementName = "NumeroEntidadesTrimestreII")]
            public string NumeroEntidadesTrimestreII { get; set; }
            [XmlElement(ElementName = "FechaTrimestreI")]
            public string FechaTrimestreI { get; set; }
            [XmlElement(ElementName = "NumeroEntidadesTrimestreI")]
            public string NumeroEntidadesTrimestreI { get; set; }
        }

        [XmlRoot(ElementName = "Endeudamiento")]
        public class Endeudamiento
        {
            [XmlElement(ElementName = "EndeudamientoTrimIII")]
            public EndeudamientoTrimIII EndeudamientoTrimIII { get; set; }
            [XmlElement(ElementName = "EndeudamientoTrimII")]
            public EndeudamientoTrimII EndeudamientoTrimII { get; set; }
            [XmlElement(ElementName = "EndeudamientoTrimI")]
            public EndeudamientoTrimI EndeudamientoTrimI { get; set; }
            [XmlElement(ElementName = "EncabezadoEndeudamiento")]
            public EncabezadoEndeudamiento EncabezadoEndeudamiento { get; set; }
        }

        [XmlRoot(ElementName = "Obligacion")]
        public class Obligacion
        {
            [XmlElement(ElementName = "NumeroMesesClausula")]
            public string NumeroMesesClausula { get; set; }
            [XmlElement(ElementName = "NumeroMesesContrato")]
            public string NumeroMesesContrato { get; set; }
            [XmlElement(ElementName = "Vigencia")]
            public string Vigencia { get; set; }
            [XmlElement(ElementName = "Reestructurado")]
            public string Reestructurado { get; set; }
            [XmlElement(ElementName = "ClausulaPermanencia")]
            public string ClausulaPermanencia { get; set; }
            [XmlElement(ElementName = "ValorCargoFijo")]
            public string ValorCargoFijo { get; set; }
            [XmlElement(ElementName = "NumeroCuotasMora")]
            public string NumeroCuotasMora { get; set; }
            [XmlElement(ElementName = "NumeroCuotasPactadas")]
            public string NumeroCuotasPactadas { get; set; }
            [XmlElement(ElementName = "EstadoTitular")]
            public string EstadoTitular { get; set; }
            [XmlElement(ElementName = "TipoPago")]
            public string TipoPago { get; set; }
            [XmlElement(ElementName = "ChequesDevueltos")]
            public string ChequesDevueltos { get; set; }
            [XmlElement(ElementName = "FechaPermanencia")]
            public string FechaPermanencia { get; set; }
            [XmlElement(ElementName = "FechaPago")]
            public string FechaPago { get; set; }
            [XmlElement(ElementName = "ModoExtincion")]
            public string ModoExtincion { get; set; }
            [XmlElement(ElementName = "FechaCorte")]
            public string FechaCorte { get; set; }
            [XmlElement(ElementName = "ParticipacionDeuda")]
            public string ParticipacionDeuda { get; set; }
            [XmlElement(ElementName = "Comportamientos")]
            public string Comportamientos { get; set; }
            [XmlElement(ElementName = "MoraMaxima")]
            public string MoraMaxima { get; set; }
            [XmlElement(ElementName = "CuotasCanceladas")]
            public string CuotasCanceladas { get; set; }
            [XmlElement(ElementName = "ValorCuota")]
            public string ValorCuota { get; set; }
            [XmlElement(ElementName = "ValorMora")]
            public string ValorMora { get; set; }
            [XmlElement(ElementName = "SaldoObligacion")]
            public string SaldoObligacion { get; set; }
            [XmlElement(ElementName = "ValorInicial")]
            public string ValorInicial { get; set; }
            [XmlElement(ElementName = "FechaApertura")]
            public string FechaApertura { get; set; }
            [XmlElement(ElementName = "Periodicidad")]
            public string Periodicidad { get; set; }
            [XmlElement(ElementName = "LineaCredito")]
            public string LineaCredito { get; set; }
            [XmlElement(ElementName = "EstadoObligacion")]
            public string EstadoObligacion { get; set; }
            [XmlElement(ElementName = "Calidad")]
            public string Calidad { get; set; }
            [XmlElement(ElementName = "NumeroObligacion")]
            public string NumeroObligacion { get; set; }
            [XmlElement(ElementName = "Sucursal")]
            public string Sucursal { get; set; }
            [XmlElement(ElementName = "Ciudad")]
            public string Ciudad { get; set; }
            [XmlElement(ElementName = "NombreEntidad")]
            public string NombreEntidad { get; set; }
            [XmlElement(ElementName = "TipoEntidad")]
            public string TipoEntidad { get; set; }
            [XmlElement(ElementName = "EstadoContrato")]
            public string EstadoContrato { get; set; }
            [XmlElement(ElementName = "TipoContrato")]
            public string TipoContrato { get; set; }
            [XmlElement(ElementName = "IdentificadorLinea")]
            public string IdentificadorLinea { get; set; }
            [XmlElement(ElementName = "PaqueteInformacion")]
            public string PaqueteInformacion { get; set; }
            [XmlElement(ElementName = "DiasCartera")]
            public string DiasCartera { get; set; }
            [XmlElement(ElementName = "Plazo")]
            public string Plazo { get; set; }
            [XmlElement(ElementName = "FechaTerminacion")]
            public string FechaTerminacion { get; set; }
            [XmlElement(ElementName = "EntidadOriginadoraCartera")]
            public string EntidadOriginadoraCartera { get; set; }
            [XmlElement(ElementName = "TipoEntidadOriginadoraCartera")]
            public string TipoEntidadOriginadoraCartera { get; set; }
            [XmlElement(ElementName = "NaturalezaReestructuracion")]
            public string NaturalezaReestructuracion { get; set; }
            [XmlElement(ElementName = "NumeroReestructuraciones")]
            public string NumeroReestructuraciones { get; set; }
            [XmlElement(ElementName = "ProbabilidadNoPago")]
            public string ProbabilidadNoPago { get; set; }
            [XmlElement(ElementName = "CubrimientoGarantia")]
            public string CubrimientoGarantia { get; set; }
            [XmlElement(ElementName = "TipoGarantia")]
            public string TipoGarantia { get; set; }
            [XmlElement(ElementName = "TipoMoneda")]
            public string TipoMoneda { get; set; }
            [XmlElement(ElementName = "Calificacion")]
            public string Calificacion { get; set; }
            [XmlElement(ElementName = "ModalidadCredito")]
            public string ModalidadCredito { get; set; }
            [XmlElement(ElementName = "ClaseTarjeta")]
            public string ClaseTarjeta { get; set; }
            [XmlElement(ElementName = "MarcaTarjeta")]
            public string MarcaTarjeta { get; set; }
        }

        [XmlRoot(ElementName = "SectorRealExtinguidas")]
        public class SectorRealExtinguidas
        {
            [XmlElement(ElementName = "Obligacion")]
            public List<Obligacion> Obligacion { get; set; }
        }

        [XmlRoot(ElementName = "SectorRealAlDia")]
        public class SectorRealAlDia
        {
            [XmlElement(ElementName = "Obligacion")]
            public List<Obligacion> Obligacion { get; set; }
        }

        [XmlRoot(ElementName = "SectorFinancieroExtinguidas")]
        public class SectorFinancieroExtinguidas
        {
            [XmlElement(ElementName = "Obligacion")]
            public List<Obligacion> Obligacion { get; set; }
        }

        [XmlRoot(ElementName = "SectorFinancieroAlDia")]
        public class SectorFinancieroAlDia
        {
            [XmlElement(ElementName = "Obligacion")]
            public List<Obligacion> Obligacion { get; set; }
        }

        [XmlRoot(ElementName = "CuentasVigentes")]
        public class CuentasVigentes
        {
            [XmlElement(ElementName = "Obligacion")]
            public List<Obligacion> Obligacion { get; set; }
        }

        [XmlRoot(ElementName = "Registro")]
        public class Registro
        {
            [XmlElement(ElementName = "ValorMora")]
            public string ValorMora { get; set; }
            [XmlElement(ElementName = "CuotaObligacionesMora")]
            public string CuotaObligacionesMora { get; set; }
            [XmlElement(ElementName = "SaldoObligacionesMora")]
            public string SaldoObligacionesMora { get; set; }
            [XmlElement(ElementName = "CantidadObligacionesMora")]
            public string CantidadObligacionesMora { get; set; }
            [XmlElement(ElementName = "CuotaObligacionesDia")]
            public string CuotaObligacionesDia { get; set; }
            [XmlElement(ElementName = "SaldoObligacionesDia")]
            public string SaldoObligacionesDia { get; set; }
            [XmlElement(ElementName = "NumeroObligacionesDia")]
            public string NumeroObligacionesDia { get; set; }
            [XmlElement(ElementName = "ParticipacionDeuda")]
            public string ParticipacionDeuda { get; set; }
            [XmlElement(ElementName = "TotalSaldo")]
            public string TotalSaldo { get; set; }
            [XmlElement(ElementName = "NumeroObligaciones")]
            public string NumeroObligaciones { get; set; }
            [XmlElement(ElementName = "PaqueteInformacion")]
            public string PaqueteInformacion { get; set; }
        }

        [XmlRoot(ElementName = "ResumenDiferentePrincipal")]
        public class ResumenDiferentePrincipal
        {
            [XmlElement(ElementName = "Registro")]
            public List<Registro> Registro { get; set; }
        }

        [XmlRoot(ElementName = "ResumenPrincipal")]
        public class ResumenPrincipal
        {
            [XmlElement(ElementName = "Registro")]
            public List<Registro> Registro { get; set; }
        }

        [XmlRoot(ElementName = "Consolidado")]
        public class Consolidado
        {
            [XmlElement(ElementName = "Registro")]
            public Registro Registro { get; set; }
            [XmlElement(ElementName = "ResumenDiferentePrincipal")]
            public ResumenDiferentePrincipal ResumenDiferentePrincipal { get; set; }
            [XmlElement(ElementName = "ResumenPrincipal")]
            public ResumenPrincipal ResumenPrincipal { get; set; }
        }

        [XmlRoot(ElementName = "Data")]
        public class Data
        {
            [XmlElement(ElementName = "RawResponse")]
            public string RawResponse { get; set; }
            [XmlElement(ElementName = "Status")]
            public string Status { get; set; }
            [XmlElement(ElementName = "GatewayTransactionId")]
            public string GatewayTransactionId { get; set; }
        }

        [XmlRoot(ElementName = "OnlineBureauDDCall")]
        public class OnlineBureauDDCall
        {
            [XmlElement(ElementName = "Success")]
            public string Success { get; set; }
            [XmlElement(ElementName = "Data")]
            public Data Data { get; set; }
        }

        [XmlRoot(ElementName = "CIFIN")]
        public class CIFIN
        {
            [XmlElement(ElementName = "Tercero")]
            public Tercero Tercero { get; set; }
            [XmlAttribute(AttributeName = "archivo")]
            public string Archivo { get; set; }
        }

        [XmlRoot(ElementName = "ResponseXML")]
        public class ResponseXML
        {
            [XmlElement(ElementName = "CIFIN")]
            public CIFIN CIFIN { get; set; }
        }

        [XmlRoot(ElementName = "DSOnlineBureauCOLData")]
        public class DSOnlineBureauCOLData
        {
            [XmlElement(ElementName = "QueueRequest")]
            public QueueRequest QueueRequest { get; set; }
            [XmlElement(ElementName = "Response")]
            public Response Response { get; set; }
            [XmlElement(ElementName = "Request")]
            public string Request { get; set; }
            [XmlElement(ElementName = "OnlineBureauDDCall")]
            public OnlineBureauDDCall OnlineBureauDDCall { get; set; }
            [XmlElement(ElementName = "ResponseXML")]
            public ResponseXML ResponseXML { get; set; }
            [XmlElement(ElementName = "IssueDate")]
            public string IssueDate { get; set; }
            [XmlElement(ElementName = "IssuePlace")]
            public string IssuePlace { get; set; }
            [XmlElement(ElementName = "IdType")]
            public string IdType { get; set; }
        }

        [XmlRoot(ElementName = "DSOnlineBureauCOLStatus")]
        public class DSOnlineBureauCOLStatus
        {
            [XmlElement(ElementName = "IsSuccess")]
            public string IsSuccess { get; set; }
            [XmlElement(ElementName = "ErrorCode")]
            public string ErrorCode { get; set; }
            [XmlElement(ElementName = "ErrorMessage")]
            public string ErrorMessage { get; set; }
            [XmlElement(ElementName = "Outcome")]
            public string Outcome { get; set; }
        }

        [XmlRoot(ElementName = "Applicant")]
        public class Applicant
        {
            [XmlElement(ElementName = "CriteriaPostBureauCOLData")]
            public CriteriaPostBureauCOLData CriteriaPostBureauCOLData { get; set; }
            [XmlElement(ElementName = "CriteriaPostBureauCOLStatus")]
            public CriteriaPostBureauCOLStatus CriteriaPostBureauCOLStatus { get; set; }
            [XmlElement(ElementName = "GrupoScore")]
            public string GrupoScore { get; set; }
            [XmlElement(ElementName = "Reasons")]
            public Reasons Reasons { get; set; }
            [XmlElement(ElementName = "CALC_PorcUtilizacion")]
            public string CALC_PorcUtilizacion { get; set; }
            [XmlElement(ElementName = "PorcEndFinal")]
            public string PorcEndFinal { get; set; }
            [XmlElement(ElementName = "PorcEndInicial")]
            public string PorcEndInicial { get; set; }
            [XmlElement(ElementName = "CapacidadEndeudamiento")]
            public string CapacidadEndeudamiento { get; set; }
            [XmlElement(ElementName = "EndeudamientoMaximo")]
            public string EndeudamientoMaximo { get; set; }
            [XmlElement(ElementName = "DisponibleFinal")]
            public string DisponibleFinal { get; set; }
            [XmlElement(ElementName = "CuotaBuro")]
            public string CuotaBuro { get; set; }
            [XmlElement(ElementName = "CuotaBuro3")]
            public string CuotaBuro3 { get; set; }
            [XmlElement(ElementName = "CuotaBuro2")]
            public string CuotaBuro2 { get; set; }
            [XmlElement(ElementName = "CuotaBuro1")]
            public string CuotaBuro1 { get; set; }
            [XmlElement(ElementName = "CuotaResto")]
            public string CuotaResto { get; set; }
            [XmlElement(ElementName = "CuotaRotativaTU3")]
            public string CuotaRotativaTU3 { get; set; }
            [XmlElement(ElementName = "CuotaRotativaTU2")]
            public string CuotaRotativaTU2 { get; set; }
            [XmlElement(ElementName = "CuotaRotativaTU1")]
            public string CuotaRotativaTU1 { get; set; }
            [XmlElement(ElementName = "CuotaTDCTU3")]
            public string CuotaTDCTU3 { get; set; }
            [XmlElement(ElementName = "CuotaTDCTU2")]
            public string CuotaTDCTU2 { get; set; }
            [XmlElement(ElementName = "CuotaTDCTU1")]
            public string CuotaTDCTU1 { get; set; }
            [XmlElement(ElementName = "CapacidadNetaAjustada")]
            public string CapacidadNetaAjustada { get; set; }
            [XmlElement(ElementName = "GastosImprevistos")]
            public string GastosImprevistos { get; set; }
            [XmlElement(ElementName = "MatrizImprevistos")]
            public string MatrizImprevistos { get; set; }
            [XmlElement(ElementName = "CapEndeudamientoNeta")]
            public string CapEndeudamientoNeta { get; set; }
            [XmlElement(ElementName = "MatrizCapEndeudamientoNeta")]
            public string MatrizCapEndeudamientoNeta { get; set; }
            [XmlElement(ElementName = "IncomeAjustadoRisk")]
            public string IncomeAjustadoRisk { get; set; }
            [XmlElement(ElementName = "MatrizAjustedeRiesgo")]
            public string MatrizAjustedeRiesgo { get; set; }
            [XmlElement(ElementName = "IncomeSMMLV")]
            public string IncomeSMMLV { get; set; }
            [XmlElement(ElementName = "InferredAge")]
            public string InferredAge { get; set; }
            [XmlElement(ElementName = "IncomeMonthlyAjustadoSMMLV")]
            public string IncomeMonthlyAjustadoSMMLV { get; set; }
            [XmlElement(ElementName = "SMMLV")]
            public string SMMLV { get; set; }
            [XmlElement(ElementName = "IncomeMonthlyAjustado")]
            public string IncomeMonthlyAjustado { get; set; }
            [XmlElement(ElementName = "IncomeEstimatorAjustado")]
            public string IncomeEstimatorAjustado { get; set; }
            [XmlElement(ElementName = "MatrizIncomeEstimatorAjustado")]
            public string MatrizIncomeEstimatorAjustado { get; set; }
            [XmlElement(ElementName = "ScoreCV")]
            public string ScoreCV { get; set; }
            [XmlElement(ElementName = "Score")]
            public string Score { get; set; }
            [XmlElement(ElementName = "IncomeEstimatorSMMLV")]
            public string IncomeEstimatorSMMLV { get; set; }
            [XmlElement(ElementName = "IncomeEstimator")]
            public string IncomeEstimator { get; set; }
            [XmlElement(ElementName = "DSCreditVisionCOLStatus")]
            public DSCreditVisionCOLStatus DSCreditVisionCOLStatus { get; set; }
            [XmlElement(ElementName = "DSCreditVisionCOLData")]
            public DSCreditVisionCOLData DSCreditVisionCOLData { get; set; }
            [XmlElement(ElementName = "DsExpertoCOLData")]
            public DsExpertoCOLData DsExpertoCOLData { get; set; }
            [XmlElement(ElementName = "SecondLastName")]
            public string SecondLastName { get; set; }
            [XmlElement(ElementName = "DsExpertoCOL")]
            public DsExpertoCOL DsExpertoCOL { get; set; }
            [XmlElement(ElementName = "IDMOutcome")]
            public string IDMOutcome { get; set; }
            [XmlElement(ElementName = "IDMReasonCodes")]
            public IDMReasonCodes IDMReasonCodes { get; set; }
            [XmlElement(ElementName = "DocumentState")]
            public string DocumentState { get; set; }
            [XmlElement(ElementName = "DSUbicaCOLStatus")]
            public DSUbicaCOLStatus DSUbicaCOLStatus { get; set; }
            [XmlElement(ElementName = "DSUbicaCOLData")]
            public DSUbicaCOLData DSUbicaCOLData { get; set; }
            [XmlElement(ElementName = "FirstLastName")]
            public string FirstLastName { get; set; }
            [XmlElement(ElementName = "SecondName")]
            public string SecondName { get; set; }
            [XmlElement(ElementName = "FirstName")]
            public string FirstName { get; set; }
            [XmlElement(ElementName = "FullName")]
            public string FullName { get; set; }
            [XmlElement(ElementName = "VelocityCheckData")]
            public VelocityCheckData VelocityCheckData { get; set; }
            [XmlElement(ElementName = "VelocityCheckStatus")]
            public VelocityCheckStatus VelocityCheckStatus { get; set; }
            [XmlElement(ElementName = "Attributes")]
            public Attributes Attributes { get; set; }
            [XmlElement(ElementName = "TipoScore")]
            public string TipoScore { get; set; }
            [XmlElement(ElementName = "IdType")]
            public string IdType { get; set; }
            [XmlElement(ElementName = "IdNumber")]
            public string IdNumber { get; set; }
            [XmlElement(ElementName = "LastName")]
            public string LastName { get; set; }
            [XmlElement(ElementName = "RecentPhoneNumber")]
            public string RecentPhoneNumber { get; set; }
            [XmlElement(ElementName = "IdExpeditionDate")]
            public string IdExpeditionDate { get; set; }
            [XmlElement(ElementName = "Occupation")]
            public string Occupation { get; set; }
            [XmlElement(ElementName = "MonthlyIncome")]
            public string MonthlyIncome { get; set; }
            [XmlElement(ElementName = "DSOnlineBureauCOLData")]
            public DSOnlineBureauCOLData DSOnlineBureauCOLData { get; set; }
            [XmlElement(ElementName = "DSOnlineBureauCOLStatus")]
            public DSOnlineBureauCOLStatus DSOnlineBureauCOLStatus { get; set; }

            [XmlElement(ElementName = "Description")]
            public string Description { get; set; }
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

        [XmlRoot(ElementName = "DinamicSelector2")]
        public class DinamicSelector2
        {
            [XmlElement(ElementName = "Variable2")]
            public string Variable2 { get; set; }
            [XmlElement(ElementName = "Condition")]
            public string Condition { get; set; }
            [XmlElement(ElementName = "Variable1")]
            public string Variable1 { get; set; }
            [XmlElement(ElementName = "BoolCondition")]
            public string BoolCondition { get; set; }
            [XmlElement(ElementName = "TypeofCondition")]
            public string TypeofCondition { get; set; }
            [XmlElement(ElementName = "NextStep")]
            public string NextStep { get; set; }
            [XmlElement(ElementName = "NextGroup")]
            public string NextGroup { get; set; }
            [XmlElement(ElementName = "PreviousStep")]
            public string PreviousStep { get; set; }
            [XmlElement(ElementName = "PreviousGroup")]
            public string PreviousGroup { get; set; }
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

        [XmlRoot(ElementName = "Step")]
        public class Step
        {
            [XmlElement(ElementName = "Duration")]
            public string Duration { get; set; }
            [XmlElement(ElementName = "Name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "DTTrail")]
        public class DTTrail
        {
            [XmlElement(ElementName = "Step")]
            public List<Step> Step { get; set; }
        }

        [XmlRoot(ElementName = "Milestone")]
        public class Milestone
        {
            [XmlElement(ElementName = "Step")]
            public List<string> Step { get; set; }
        }

        [XmlRoot(ElementName = "ApplicationData")]
        public class ApplicationData
        {
            [XmlElement(ElementName = "OriginalSkipDSTUBureauCOLFlag")]
            public string OriginalSkipDSTUBureauCOLFlag { get; set; }
            [XmlElement(ElementName = "UltimaEtapaFlujo")]
            public string UltimaEtapaFlujo { get; set; }
            [XmlElement(ElementName = "secuenciaTercero")]
            public string SecuenciaTercero { get; set; }
            [XmlElement(ElementName = "CreateHour")]
            public string CreateHour { get; set; }
            [XmlElement(ElementName = "test")]
            public List<string> Test { get; set; }
            [XmlElement(ElementName = "DecisionHomologada")]
            public string DecisionHomologada { get; set; }
            [XmlElement(ElementName = "Decision")]
            public string Decision { get; set; }
            [XmlElement(ElementName = "Description")]
            public string Description { get; set; }
            [XmlElement(ElementName = "PolicyPostBureauComplete")]
            public string PolicyPostBureauComplete { get; set; }
            [XmlElement(ElementName = "PostBureauReasons")]
            public PostBureauReasons PostBureauReasons { get; set; }
            [XmlElement(ElementName = "PostBureuReferCount")]
            public string PostBureuReferCount { get; set; }
            [XmlElement(ElementName = "PostBureuFailCount")]
            public string PostBureuFailCount { get; set; }
            [XmlElement(ElementName = "HoraConsulta")]
            public string HoraConsulta { get; set; }
            [XmlElement(ElementName = "FechaConsulta")]
            public string FechaConsulta { get; set; }
            [XmlElement(ElementName = "DsExpertoReasonsCode")]
            public DsExpertoReasonsCode DsExpertoReasonsCode { get; set; }
            [XmlElement(ElementName = "InvalidDocumentState")]
            public string InvalidDocumentState { get; set; }
            [XmlElement(ElementName = "IDVReasonsCode")]
            public IDVReasonsCode IDVReasonsCode { get; set; }
            [XmlElement(ElementName = "ReturnMessage")]
            public string ReturnMessage { get; set; }
            [XmlElement(ElementName = "porcentajeApellido")]
            public string PorcentajeApellido { get; set; }
            [XmlElement(ElementName = "GenerarReporte")]
            public string GenerarReporte { get; set; }
            [XmlElement(ElementName = "XMLFile")]
            public string XMLFile { get; set; }
            [XmlElement(ElementName = "HTMLFile")]
            public string HTMLFile { get; set; }
            [XmlElement(ElementName = "XSLFile")]
            public string XSLFile { get; set; }
            [XmlElement(ElementName = "VelocityCheckReasons")]
            public VelocityCheckReasons VelocityCheckReasons { get; set; }
            [XmlElement(ElementName = "Permutacion")]
            public string Permutacion { get; set; }
            [XmlElement(ElementName = "DinamicSelector")]
            public DinamicSelector DinamicSelector { get; set; }
            [XmlElement(ElementName = "DTTrail")]
            public DTTrail DTTrail { get; set; }
            [XmlElement(ElementName = "InputValReasonCodes")]
            public string InputValReasonCodes { get; set; }
            [XmlElement(ElementName = "Start")]
            public string Start { get; set; }
            [XmlElement(ElementName = "Milestone")]
            public Milestone Milestone { get; set; }
            [XmlElement(ElementName = "TipoScore")]
            public string TipoScore { get; set; }
            [XmlElement(ElementName = "ReportDays")]
            public string ReportDays { get; set; }
            [XmlElement(ElementName = "MaxNumberPendingRules")]
            public string MaxNumberPendingRules { get; set; }
            [XmlElement(ElementName = "TypeofCuota")]
            public string TypeofCuota { get; set; }
            [XmlElement(ElementName = "FeeCalculation")]
            public string FeeCalculation { get; set; }
            [XmlElement(ElementName = "MaximumUtilizationPercentage")]
            public string MaximumUtilizationPercentage { get; set; }
            [XmlElement(ElementName = "MinimumFee")]
            public string MinimumFee { get; set; }
            [XmlElement(ElementName = "EndPercentageMaximum")]
            public string EndPercentageMaximum { get; set; }
            [XmlElement(ElementName = "RotatingFactor")]
            public string RotatingFactor { get; set; }
            [XmlElement(ElementName = "FactorTDC")]
            public string FactorTDC { get; set; }
            [XmlElement(ElementName = "SMMLV")]
            public string SMMLV { get; set; }
            [XmlElement(ElementName = "FlowType")]
            public string FlowType { get; set; }
            [XmlElement(ElementName = "CreateDate")]
            public string CreateDate { get; set; }
            [XmlElement(ElementName = "SkipFlagFacturacionOnline")]
            public string SkipFlagFacturacionOnline { get; set; }
            [XmlElement(ElementName = "SkipDSTUBureauCOLFlag")]
            public string SkipDSTUBureauCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipBillingFlag")]
            public string SkipBillingFlag { get; set; }
            [XmlElement(ElementName = "SkipAffordabilityCalcCOLFlag")]
            public string SkipAffordabilityCalcCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipIncomeValidatorFlag")]
            public string SkipIncomeValidatorFlag { get; set; }
            [XmlElement(ElementName = "SkipCreditVisionFlag")]
            public string SkipCreditVisionFlag { get; set; }
            [XmlElement(ElementName = "SkipCriteriaPostBureauCOLFlag")]
            public string SkipCriteriaPostBureauCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipLookupInternalFlag")]
            public string SkipLookupInternalFlag { get; set; }
            [XmlElement(ElementName = "SkipDSUbicaCOLFlag")]
            public string SkipDSUbicaCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipDsExpertoEntidadCOLFlag")]
            public string SkipDsExpertoEntidadCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipDsExpertoCOLFlag")]
            public string SkipDsExpertoCOLFlag { get; set; }
            [XmlElement(ElementName = "SkipVelocityCheckFlag")]
            public string SkipVelocityCheckFlag { get; set; }
            [XmlElement(ElementName = "EnvironmentType")]
            public string EnvironmentType { get; set; }
            [XmlElement(ElementName = "EnvironmentTypeId")]
            public string EnvironmentTypeId { get; set; }
            [XmlElement(ElementName = "SolutionSetId")]
            public string SolutionSetId { get; set; }
            [XmlElement(ElementName = "ApplicationId")]
            public string ApplicationId { get; set; }
            [XmlElement(ElementName = "BusinessUnitId")]
            public string BusinessUnitId { get; set; }
            [XmlElement(ElementName = "User")]
            public string User { get; set; }
            [XmlElement(ElementName = "Product")]
            public string Product { get; set; }
            [XmlElement(ElementName = "actualDate")]
            public string ActualDate { get; set; }
            [XmlElement(ElementName = "Hours")]
            public string Hours { get; set; }            
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
