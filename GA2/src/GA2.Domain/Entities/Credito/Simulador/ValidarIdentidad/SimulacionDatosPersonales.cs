using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Tabla Simulacion Datos Personales
    /// </summary>
    /// <author>German Eduardo Guarnizo</author>
    /// <date>26/03/2021</date>
    public class SimulacionDatosPersonales
    {
        public Guid SDP_ID { get; set; }
        public Guid SLS_ID { get; set; }
        public int FRZ_ID { get; set; }
        public int CTG_ID { get; set; }
        public int GRD_ID { get; set; }
        public string SDP_NUMERO_DOCUMENTO { get; set; }
        public string SDP_NOMBRES_APELLIDOS { get; set; }
        public int SDP_EDAD { get; set; }
        public DateTime SDP_FECHA_AFILIACION { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public string SDP_DIRECCION { get; set; }
        public string SDP_TELEFONO_FIJO { get; set; }
        public string SDP_TELEFONO_CELULAR { get; set; }
        public string SDP_E_MAIL { get; set; }
        public int SDP_CUOTAS { get; set; }
        public string SDP_REGIMEN { get; set; }
        public decimal SDP_VALOR_INMUEBLE { get; set; }
        public decimal SDP_VALOR_AVALUO_INMUEBLE { get; set; }
        public decimal SMC_VALOR_VIVIENDA_SEGURO { get; set; }
        public int TVV_ID { get; set; }
        public int DPD_ID_INMUEBLE { get; set; }
        public int DPC_ID_INMUEBLE { get; set; }
        public Guid SDP_USUARIO_ACTUALIZA { get; set; }
        public DateTime SDP_FECHA_ACTUALIZA { get; set; }
        public string DEPARTAMENTO_RESIDENCIA { get; set; }
        public string CIUDAD_RESIDENCIA { get; set; }
        public string DEPARTAMENTO_INMIUEBLE { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public string FUERZA { get; set; }
        public string CATEGORIA { get; set; }
        public string GRADO { get; set; }
        public string TIPO_VIVIENDA { get; set; }
        public int TPA_ID { get; set; }
        public string TIPO_ACTOR { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public object UEJ_ID { get; set; }
        public decimal SMC_VALOR_PRESTAMO_SOLICITADO { get; set; }
        public decimal SMC_PORCEN_OPCION_DE_COMPRA { get; set; }
        public decimal SMC_PORCEN_CANON_INICIAL { get; set; }
        public int SMC_TASA_VOLUNTARIA { get; set; }
        public Guid? USR_ID { get; set; }
        public bool SDP_COMPRA_CARTERA { get; set; }
        public string SMC_VIVIENDA_VIS { get; set; }
        public int? CLC_ID { get; set; }
        public Guid SMC_ID { get; set; }
        public decimal SMC_TASA_SEGURO_HOGAR { get; set; }
        public decimal SMC_TASA_SEGURO_VIDA { get; set; }
        public decimal SMC_VALOR_TASA_EA { get; set; }
        public int DEPARTAMENTO_ID { get; set; }
        public int CIUDAD_ID { get; set; }
        public bool SDP_OPCION_COMPRA_CARTERA { get; set; }
        public decimal SDP_VALOR_PRESTAMO_COMPRA_CARTERA { get; set; }
        public string SDP_NUMERO_OBLIGACION_COMPRA_CARTERA { get; set; }
    }
}
