using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class SolicitudCredito
    {
        public Guid SOC_ID { get; set; }
        public DateTime SOC_FECHA_SOLICITUD { get; set; }
        public int SOC_NUMERO_SOLICITUD { get; set; }
        public int TCR_ID { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public int DPD_ID_INMUEBLE { get; set; }
        public int DPC_ID_INMUEBLE { get; set; }
        public Guid SLS_ID { get; set; }
        public int TID_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string SOC_DECLARACION_ORIGEN_FONDOS { get; set; }
        public string SOC_AUTORIZA_USO_DATOS { get; set; }
        public string SOC_AUTORIZA_CONSULTA_CENTRALES { get; set; }
        public string SOC_DECLARACION_ASEGURABILIDAD { get; set; }
        public string SOC_ESTADO { get; set; }
        public string SOC_CONVENIO_ASEGURADORA { get; set; }
        public string SOC_CONVENIO_ASEGURADORA_HOGAR { get; set; }
        public int ASE_ID { get; set; }
        public int ASE_ID_HOGAR { get; set; }
        public string SOC_DECISION_BURO { get; set; }
        public string SOC_SCORE { get; set; }
        public decimal SOC_CAPACIDAD_ENDEUDAMIENTO { get; set; }
        public decimal SOC_NIVEL_ENDEUDAMIENTO { get; set; }
        public decimal SOC_NIVEL_ENDEUDAMIENTO_CUOTA { get; set; }
        public decimal SOC_PORC_EXTRAPRIMA { get; set; }

        public decimal SOC_TASA_FECH { get; set; }
        public decimal SOC_VALOR_ALIVIO { get; set; }
        public Guid SOC_CREADO_POR { get; set; }
        public DateTime SOC_FECHA_CREACION { get; set; }
        public Guid SOC_ACTUALIZADO_POR { get; set; }
        public DateTime SOC_FECHA_ACTUALIZA { get; set; }
        public string SOC_NUMERO_RADICADO { get; set; }
        public Int32? MPR_CONT { get; set; }
        public Int32? MPR_CONT_SP { get; set; }
        public string FAC_CONT { get; set; }
        public string FAC_NUME { get; set; }
        public Int32? PVD_CODD { get; set; }
        public Guid SOP_ID { get; set; }
        public string SOP_PROPIEDAD_HORIZONTAL { get; set; }
        public string SOP_TIPO_DEPOSITO { get; set; }
        public string SOP_TIPO_GARAGE { get; set; }
        public string SOP_ESTADO_INMUEBLE { get; set; }
        public string SOP_TIPO_INMUEBLE { get; set; }
        public string SOP_TIENE_GARAJE { get; set; }
        public string SOP_TIENE_DEPOSITO { get; set; }
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
        public int SOP_PISO_INMUEBLE { get; set; }
        public string SOP_POSEE_ASENSOR { get; set; }
        public decimal SOP_VALOR_CREDITO { get; set; }
        public decimal SOP_PORC_FINANCIACION { get; set; }
        public int SOP_PLAZO_FINANCIACION { get; set; }
        public string SOP_NUMERO_DOCUMENTO_CONSTRUCTOR { get; set; }
        public string SOP_NOMBRE_CONSTRUCTOR { get; set; }
        public DateTime? SOP_FECHA_ENTREGA_INMUEBLE { get; set; }
        public string SOP_NOMBRE_FIDUCIA { get; set; }
        public string SOP_NOMBRE_PROYECTO { get; set; }
        public int TID_ID_VENDEDOR { get; set; }
        public int TID_ERP { get; set; }
        public int TID_EMPRESARIAL { get; set; }
        public string SOP_NUMERO_DOCUMENTO_VENDEDOR { get; set; }
        public string SOP_NOMBRE_VENDEDOR { get; set; }
        public string SOP_DIRECCION_VENDEDOR { get; set; }
        public string DPD_CODIGO { get; set; }
        public int DPD_ID_VENDEDOR { get; set; }
        public int DPC_ID_VENDEDOR { get; set; }
        public int DPC_CODIGO { get; set; }
        public int DPP_CODIGO { get; set; }
        public string SOP_CORREO_VENDEDOR { get; set; }
        public string SOP_TELEFONO_VENDEDOR { get; set; }
        public Guid SOP_CREADO_POR { get; set; }
        public DateTime SOP_FECHA_CREACION { get; set; }
        public Guid SOP_ACTUALIZADO_POR { get; set; }
        public DateTime SOP_FECHA_ACTUALIZA { get; set; }
        public string SOC_SOCRE { get; set; }
        public string SLS_NUMERO_RADICADO { get; set; }
        public Guid? USR_ID { get; set; }
        public int DPD_DEPARTAMENTO_NOTARIA { get; set; }
        public int DPC_CIUDAD_NOTARIA { get; set; }
        public int SOC_SCORE_TU { get; set; }
        public Guid TRS_ID { get; set; }

        //CAMPOS PARA FIDUCIA
        public string FID_ACTIVO { get; set; }
        public int FID_TIPO_DOCUMENTO { get; set; }
        public string FID_NUMERO_DOCUMENTO { get; set; }
        public string FID_RAZON_SOCIAL { get; set; }
        public string FID_DIRECCION { get; set; }
        public int FID_DEPARTAMENTO { get; set; }
        public int FID_CIUDAD { get; set; }
        public string FID_CORREO { get; set; }
        public string FID_TELEFONO { get; set; }

    }
}
