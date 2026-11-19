using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Entidad educativa
    /// </summary>
    public class EntidadEducativa
    {

        public Guid ENE_ID { get; set; }
        public int ENE_TIPO_IDENTIFICACION { get; set; }
        public string ENE_NIT { get; set; }
        public string ENE_NOMBRE_RAZON_SOCIAL { get; set; }
        public string ENE_CORREO_ELECTRONICO { get; set; }
        public int ENE_DPC_ID { get; set; }
        public string ENE_DIRECCION { get; set; }
        public string ENE_TELEFONO { get; set; }
        public string ENE_NOMBRE_CONTACTO { get; set; }
        public bool ENE_ESTADO{ get; set; }
        public string ENE_NOMBRE_CIUDAD { get; set; }
        public string ENE_NOMBRE_DEPARTAMENTO { get; set; }
        public int DPD_ID { get; set; }
        public int ENE_CVL_ACREDITADA_ID { get; set; }
        public int ENE_CVL_TIPO_PERSONA_ID { get; set; }
        public Guid ENE_CREADOPOR { get; set; }
        public DateTime ENE_FECHA_CREACION { get; set; }
        public Guid ENE_MODIFICADOPOR { get; set; }
        public DateTime ENE_FECHA_MODIFICACION { get; set; }
        public DateTime ENE_PAM_FECHACREACION { get; set; }
        public bool ENE_BLOQUEO { get; set; }
        public int RENOVAR_ARCHIVO { get; set; }
        public bool SOLICITUDES_PROCESADAS { get; set; }

    }
    public class Fideicomiso
    {
        public Guid? ENE_FIDEICOMISO_ID { get; set; }
        public Guid ENE_ID { get; set; }
        public Guid? ENEC_USER { get; set; }
        public string ENE_TIPO_IDENTIFICACION { get; set; }
        public string ENE_NUMERO_IDENTIFICACION { get; set; }
        public string ENE_RAZON_SOCIAL_FIDEICOMISO { get; set; }
        public DateTime? ENE_FEC_CRE { get; set; }
        public string TipoDocumento { get; set; }
    }
}