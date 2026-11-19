using System;

namespace GA2.Domain.Entities
{
    public class SimulacionCliente
    {
        public string SMC_NUMERO_DOCUMENTO { get; set; }
        public string SMC_NUMERO_PREAPROBADO { get; set; }
        public Guid SMC_ID { get; set; }
        public string SMC_TIPO_VIVIENDA { get; set; }
        public decimal SMC_VALOR_VIVIENDA { get; set; }
        public decimal SMC_VALOR_VIVIENDA_SEGURO { get; set; }
        public decimal SMC_VALOR_PRESTAMO { get; set; }
        public string SMC_TIPO_ALIVIO { get; set; }
        public string SMC_TIPO_ABONO { get; set; }
        public decimal SMC_VALOR_TASA_EA { get; set; }
        public decimal SMC_VALOR_TASA_MV { get; set; }
        public decimal SMC_VALOR_TASA_MV_FRESH { get; set; }
        public string SMC_VIVIENDA_VIS { get; set; }
        public decimal SMC_VALOR_CUOTA { get; set; }
        public decimal SMC_VALOR_CUOTA_SIN_SEG { get; set; }
        public int SMC_PLAZO { get; set; }
        public DateTime SMC_FECHA_SIMULACION { get; set; }
        public DateTime SMC_FECHA_APROBACION { get; set; }
        public string NOMBRE_CLIENTE { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string CIUDAD { get; set; }
        public string DIRECCION { get; set; }
        public string CORREO_ELECTRONICO { get; set; }
        public string TELEFONO_FIJO { get; set; }
        public string TELEFONO_CELULAR { get; set; }
        public string FUERZA { get; set; }
        public string REGIMEN { get; set; }
        public string CATEGORIA { get; set; }
        public string GRADO { get; set; }
        public int TIPO_CREDITO { get; set; }
        public double AÑOS { get; set; }
        public string TIPO_ABONO { get; set; }
        public string TIPO_VIVIENDA { get; set; }
        public int DPC_INMUEBLE { get; set; }
        public int DPD_INMUEBLE { get; set; }
        public int TIV_VIVIENDA_ID { get; set; }
        public decimal VALOR_CUOTA_MOD { get; set; }
        public int UEJ_ID { get; set; }
        public decimal SMC_PORCE_TASAFRES { get; set; }
        public Guid MEDIO_ATENCION_CREDITO { get; set; }
        public decimal SMC_PORCEN_CANON_INICIAL { get; set; }
    }

    public class PreaprobadoCliente
    {
        public decimal SMD_CUOTA_TOTAL { get; set; }
        public string SMC_NUMERO_PREAPROBADO { get; set; }
        public string SMC_NUMERO_DOCUMENTO { get; set; }
        public decimal SMC_VALOR_CUOTA { get; set; }
        public decimal SMC_VALOR_PRESTAMO { get; set; }
        public int SMC_PLAZO { get; set; }
        public decimal VALOR_CUOTA_MOD { get; set; }
        public string NUMERO_RADICADO { get; set; }

        public DateTime FECHA_ACTUALIZA { get; set; }

        public string MODALIDAD_CREDITO { get; set; }

        public string PUNTO_ATENCION { get; set; }
        public string SLS_NUMERO_RADICADO_83 { get; set; }

        public string SLS_SIMULADOR_DESC { get; set; }
    }
}
