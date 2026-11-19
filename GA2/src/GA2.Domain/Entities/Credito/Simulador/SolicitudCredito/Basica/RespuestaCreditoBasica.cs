using System;

namespace GA2.Domain.Entities
{
    public class RespuestaCreditoBasica
    {
        public Guid SOB_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public int TID_ID { get; set; }
        public string SOB_NUMERO_DOCUMENTO { get; set; }
        public DateTime SOB_FECHA_EXPEDICION { get; set; }
        public int DPD_ID_EXP { get; set; }
        public int DPC_ID_EXP { get; set; }
        public string SOB_PRIMER_APELLIDO { get; set; }
        public string SOB_SEGUNDO_APELLIDO { get; set; }
        public string SOB_PRIMER_NOMBRE { get; set; }
        public string SOB_SEGUNDO_NOMBRE { get; set; }
        public DateTime SOB_FECHA_NACIMIENTO { get; set; }
        public int DPD_ID_NACIMIENTO { get; set; }
        public int DPC_ID_NACIMIENTO { get; set; }
        public string SOB_SEXO { get; set; }
        public string SOB_ESTADO_CIVIL { get; set; }
        public string SOB_NIVEL_EDUCACION { get; set; }
        public int FRZ_ID { get; set; }
        public int CTG_ID { get; set; }
        public int GRD_ID { get; set; }
        public int DPD_ID_RESIDENCIA { get; set; }
        public int DPC_ID_RESIDENCIA { get; set; }
        public string SOB_DIRECCION_RESIDENCIA { get; set; }
        public string SOB_CORREO_PERSONAL { get; set; }
        public string SOB_TELEFONO_RESIDENCIA { get; set; }
        public string SOB_CELULAR { get; set; }
        public int DPD_ID_OFICINA { get; set; }
        public int DPC_ID_OFICINA { get; set; }
        public string SOB_DIRECCION_OFICINA { get; set; }
        public string SOB_CORREO_INSTITUCIONAL { get; set; }
        public string SOB_TELEFONO_OFICINA { get; set; }
        public int ACC_ID { get; set; }
        public int PRF_ID { get; set; }
        public string SOB_TIENE_TRANSACCION_ME { get; set; }
        public string SOB_TIPO_TRANSACCION_ME { get; set; }
        public string SOB_TIPO_PRODUCTO_ME { get; set; }
        public string SOB_BANCO_TRANSACCION_ME { get; set; }
        public string SOB_NUMERO_PRODUCTO_ME { get; set; }
        public int SOB_SALDO_PROMEDIO_CUENTAS_ME { get; set; }
        public string SOB_MONEDA_TRANSACCION_ME { get; set; }
        public int DPP_ID_ME { get; set; }
        public int DPD_ID_ME { get; set; }
        public Guid SOB_CREADO_POR { get; set; }
        public Guid SOB_ACTUALIZADO_POR { get; set; }
        public DateTime SOB_FECHA_CREACION { get; set; }
        public DateTime SOB_FECHA_ACTUALIZA { get; set; }
        public string DEPARTAMENTO_EXPEDICION { get; set; }
        public string CIUDAD_EXPEDICION { get; set; }
        public string DEPARTAMENTO_NACIMIENTO { get; set; }
        public string CIUDAD_NACIMIENTO { get; set; }
        public string FUERZA { get; set; }
        public string CATEGORIA { get; set; }
        public string GRADO { get; set; }
        public string SEXO { get; set; }
        public string NIVEL_EDUCATIVO { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public string TIPO_DOCUMENTO_CLIENTE { get; set; }
        public string PROFESION { get; set; }
        public DateTime FECHA_AFILIACION { get; set; }
        public string NUMERO_RADICADO { get; set; }
        public string CIUDAD_SOLICITUD { get; set; }
        public DateTime FECHA_SOLICITUD { get; set; }
        public int TIPO_CREDITO { get; set; }
        public string CREDITO_HIPOTECARIO { get; set; }
        public string CREDITO_LEASING { get; set; }
        public string CIUDAD_RESIDENCIA { get; set; }
        public string DEPARTAMENTO_RESIDENCIA { get; set; }
        public string ACTIVIDAD { get; set; }
        public string EMPRESA_TRABAJA { get; set; }
        public string DEPARTAMENTO_OFICINA { get; set; }
        public string CIUDAD_OFICINA { get; set; }
        public string MEDIO_CONTACTO { get; set; }
        public string TIPO_IDENTIFICACION_CONYUGUE { get; set; }
        public string CIUDAD_EXPEDICION_CONYUGUE{ get; set; }
        public string CIUDAD_REFERENCIA_FAMILIAR{ get; set; }
        public string CIUDAD_REFERENCIA_PERSONAL { get; set; }
        public decimal OTROS_INGRESOS { get; set; }
        public string MONEDA_EXTRANJERA_SI { get;set; }
        public string MONEDA_EXTRANJERA_NO { get; set; }
        public string TIPO_TRANSACION_DESCRIPCION { get; set; }
        public string TIPO_PRODUCTO_TRANSACCIONES{ get; set; }
        public string PAIS_DESCRIPCION{ get; set; }
        public string VIS { get; set; }
        public string NO_VIS { get; set; }
        public string TIPO_VIVIENDA_NUEVA { get; set; }
        public string TIPO_VIVIENDA_USADA { get; set; }

        public string TIENE_GARAJE { get; set; }
        public string NO_TIENE_GARAJE { get; set; }
        public string TIENE_ASCENSOR { get; set; }
        public string NO_TIENE_ASCENSOR { get; set; }
        public string NOTARIA { get; set; }

        public DateTime FECHA_ENTREGA { get; set; }



    }
}
