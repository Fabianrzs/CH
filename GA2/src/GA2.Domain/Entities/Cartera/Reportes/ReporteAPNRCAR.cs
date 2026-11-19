using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ReporteAPNRCAR
    {
        public long CONSECUTIVO_NUMERO_DE_CONTRATO { get; set; }
        public int CREDITO_INVERSION_INTERBANCARIO { get; set; }
        public int CLASE_DE_CREDITO_O_INVERSION { get; set; }
        public int CLASIFICACION_Y_PONDERACION { get; set; }
        public string NIT { get; set; }
        public string RIESGO_CORTO_PLAZO { get; set; }
        public string RIESGO_LARGO_PLAZO { get; set; }
        public string INVERSION_SUBORDINADA { get; set; }
        public int DIAS_RESIDUAL { get; set; }
        public string EFECTIVO { get; set; }
        public int GARANTIZADOS_POR_O_EMITIDO_POR { get; set; }
        public int DESTINO_DEL_CREDITO { get; set; }
        public string DESEMBOLSO { get; set; }
        public string DESTINO_DE_OTORGAMIENTO { get; set; }
        public string REVOCABLE { get; set; }
        public int PROCESOS_DE_TITULARIZACION { get; set; }
        public decimal PROVISION { get; set; }
        public decimal COBERTURA_DE_LA_GARANTIA_VALORACION { get; set; }
        public int CLASE_DE_GARANTIA { get; set; }
        public string GARANTIA_VIVIENDA { get; set; }
        public int CRE_VALOR_VIVIENDA { get; set; }
        public int DIAS_DE_INCUMPLIMIENTO { get; set; }
        public int TIPO_DE_CARTERA { get; set; }
        public string MONEDA_LOCAL { get; set; }
        public string ACTIVO_EN_INCUMPLIMIENTO { get; set; }
        public DateTime FECHA_VENCIMIENTO { get; set; }
        public DateTime FECHA_INICIAL { get; set; }
        public int PLAZO_INICIAL { get; set; }
        public DateTime FECHA_CORTE { get; set; }
        public string CODIGO_GARANTIA { get; set; }
        public decimal VALOR_DE_LA_GARANTIA_DEL_CREDITO { get; set; }
        public decimal SALDO_CREDITO { get; set; }
        public decimal SALDO_CUENTA_POR_COBRAR { get; set; }
        public decimal PORCENTAJE_DE_GARANTIA { get; set; }
        public Guid CRE_ID { get; set; }

    }
}
