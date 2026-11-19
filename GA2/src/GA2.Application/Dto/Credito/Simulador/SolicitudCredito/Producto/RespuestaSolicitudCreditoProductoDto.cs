using System;

namespace GA2.Application.Dto
{
    public class RespuestaSolicitudCreditoProductoDto
    {
        public Guid SOP_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public string SOP_ESTADO_INMUEBLE { get; set; }
        public string SOP_TIPO_INMUEBLE { get; set; }
        public string SOP_TIENE_GARAJE { get; set; }
        public string SOP_TIENE_DEPOSITO { get; set; }
        public string SOP_PROPIEDAD_HORIZONTAL { get; set; }
        public string SOP_TIPO_DEPOSITO { get; set; }
        public string SOP_TIPO_GARAGE { get; set; }
        public string SOP_CONJUNTO_CERRADO { get; set; }
        public int TVV_ID { get; set; }
        public int SOP_AÑOS_CONSTRUCCION { get; set; }
        public string SOP_MATRICULA_INMOBILIARIA1 { get; set; }
        public string SOP_MATRICULA_INMOBILIARIA2 { get; set; }
        public string SOP_MATRICULA_INMOBILIARIA3 { get; set; }
        public decimal SOP_VALOR_INMUEBLE { get; set; }
        public string SOP_DIRECCION_INMUEBLE { get; set; }
        public string SOP_BARRIO_INMUEBLE { get; set; }
        public string SOP_LOCALIDAD_INMUEBLE { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public decimal SOP_VALOR_CREDITO { get; set; }
        public int SOP_PORC_FINANCIACION { get; set; }
        public int SOP_PLAZO_FINANCIACION { get; set; }
        public int TID_ID { get; set; }
        public string SOP_NUMERO_DOCUMENTO_CONSTRUCTOR { get; set; }
        public DateTime SOP_FECHA_ENTREGA_INMUEBLE { get; set; }
        public string SOP_NOMBRE_FIDUCIA { get; set; }
        public string SOP_NOMBRE_PROYECTO { get; set; }
        public int TID_ID_VENDEDOR { get; set; }
        public string SOP_NUMERO_DOCUMENTO_VENDEDOR { get; set; }
        public string SOP_NOMBRE_VENDEDOR { get; set; }
        public string SOP_DIRECCION_VENDEDOR { get; set; }
        public int DPD_ID_VENDEDOR { get; set; }
        public int DPC_ID_VENDEDOR { get; set; }
        public string SOP_CORREO_VENDEDOR { get; set; }
        public string SOP_TELEFONO_VENDEDOR { get; set; }
        public Guid SOP_CREADO_POR { get; set; }
        public DateTime SOP_FECHA_CREACION { get; set; }
        public Guid SOP_ACTUALIZADO_POR { get; set; }
        public DateTime SOP_FECHA_ACTUALIZA { get; set; }
        public string DEPARTAMENTO_INMUEBLE { get; set; }
        public string TIPO_DOCUMENTO_VENDEDOR { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public string TIPO_VIVIENDA { get; set; }
        public string TIPO_INMUEBLE { get; set; }
        public string NOMBRE_TIPO_VIVIENDA { get; set; }
        public decimal CANON_INICIAL { get; set; }
        public decimal TASA_ANUAL { get; set; }
        public decimal TASA_MENSUAL { get; set; }
        public string NUMERO_PREAPROBADO { get; set; }
        public decimal CUOTA_TOTAL_SIN_SEGUROS { get; set; }
        public decimal CUOTA_TOTAL { get; set; }
        public decimal PORCENTAJE_FINANCIACION { get; set; }
        public DateTime fechaDesembolso { get; set; }
    }
}
