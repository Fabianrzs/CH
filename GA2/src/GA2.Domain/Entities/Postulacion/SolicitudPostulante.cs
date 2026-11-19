using System;

namespace GA2.Domain.Entities
{
    public class SolicitudPostulante
    {
      
        public Guid SPO_ID_SOLICITUD { get; set; }
        public int SPO_TIPO_DOCUMENTO_ID { get; set; }
        public long SPO_NUMERO_DOCUMENTO { get; set; }
        public int SPO_ID_TIPO_DE_SOLICITUD { get; set; }
        public int SPO_ID_MODALIDAD { get; set; }
        public int SPO_ID_SUBMODALIDAD { get; set; }
        public int SPO_MANIFESTACION_AUTORIZACIONES { get; set; }
        public int SPO_DEPARTAMENTO_PREFERENCIA1 { get; set; }
        public int SPO_CIUDAD_PREFERENCIA1 { get; set; }
        public int SPO_DEPARTAMENTO_PREFERENCIA2 { get; set; }
        public int SPO_CIUDAD_PREFERENCIA2 { get; set; }
        public int SPO_DEPARTAMENTO_PREFERENCIA3 { get; set; }
        public int SPO_CIUDAD_PREFERENCIA3 { get; set; }
        public int SPO_TIPO_DOCUMENTO_FIRMA_SOLICITUD { get; set; }
        public long SPO_NUMERO_DOCUMENTO_FIRMA_SOLICITUD { get; set; }
        public DateTime? SPO_FECHAEXPEDICION { get; set; }
        public long SPO_NUMERO_CELULAR { get; set; }
        public long SPO_NUMERO_CONVOCATORIA { get; set; }
        public int SPO_ESTADO { get; set; }
        public int ACTIVIDA_POSTULACION { get; set; }
        public string SPO_OBSERVACION { get; set; }
        public string SPO_NOMBRES_FIRMA { get; set; }
        public string SPO_APELLIDOS_FIRMA { get; set; }
        public DateTime SPO_FECHA_CREACION { get; set; }
        public bool SPO_REPRESENTANTE_LEGAL { get; set; }
        public string SPO_DISFRUTE_PENSION { get; set; }
        public string SPO_UNIDAD_EJECUTORA { get; set; }
        public string SPO_CATEGORIA { get; set; }
        public string SPO_NOMBRE_AFILIADO_PRINCIPAL { get; set; }
        public Guid SPO_USERID { get; set; }
        public string TipoSolicitud { get; set; }
        public string ModalidadSolucionVivienda { get; set; }
        public string AfectacionCuenta { get; set; }
        public double ValorAfectacion { get; set; }
        public string Fuerza { get; set; }
        public int IdConvocatoria { get; set; }

        public DateTime? SPO_FECHA_RETIRO_FUERZA { get; set; } = null;
        public DateTime? SPO_FECHA_RETIRO { get; set; } = null;

        public DateTime? SPO_FECHA_ASIGNACION_INVALIDEZ { get; set; } = null;

        public DateTime? SPO_FECHA_ { get; set; } = null;
        public Boolean SPO_MISMO_AFILIADO { get; set; }
        public Boolean? SPO_PRINCIPAL { get; set; } = null;

        public DateTime? SPO_FECHA_PRIMER_APORTE { get; set; } = null;

        public DateTime? SPO_FECHA_ULTIMO_APORTE { get; set; } = null;
        public DateTime? SPO_FECHA_PRIMER_ULTIMO_APORTE { get; set; } = null;

        public DateTime? SPO_FECHA_SEGUNDO_ULTIMO_APORTE { get; set; } = null;
        public Guid? SPO_ID_SOLICITUD_PRINCIPAL { get; set; }
    }
}
