using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.ProductoPorCliente
{
    public class CrearProductoCliente
    {
        public Guid CRE_ID { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public decimal CRE_VALOR_DESEMBOLSO { get; set; }
        public decimal CRE_VALOR_CUOTA { get; set; }
        public int CRE_PLAZO_TOTAL { get; set; }
        public int TCR_ID { get; set; }
        public int CRE_DIA_PAGO { get; set; }
        public int ESC_ID { get; set; }
        public DateTime CRE_FECHA_ESTADO { get; set; }
        public int CRE_DIAS_MORA { get; set; }
        public decimal HAG_VALOR_GARANTIA { get; set; }
        public decimal CRE_VALOR_VIVIENDA { get; set; }
        public decimal CRE_VALOR_VIVIENDA_SEGURO_HOGAR { get; set; }
        public int TIV_ID { get; set; }
        public int TVV_ID { get; set; }
        public decimal CRE_SALDO_CAPITAL { get; set; }
        public decimal CRE_SALDO_CAPITAL_MORA { get; set; }
        public int CRE_PLAZO_ACTUAL { get; set; }
        public decimal CRE_TASA_SEGURO_VIDA { get; set; }
        public decimal CRE_TASA_SEGURO_HOGAR { get; set; }
        public decimal CRE_TASA_CREDITO_EA { get; set; }
        public double CRE_TASA_CREDITO_PER { get; set; }
        public decimal CRE_TASA_FRECH { get; set; }
        public decimal CRE_VALOR_ALIVIO_CUOTA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO { get; set; }
        public DateTime CRE_FECHA_PROXIMO_PAGO { get; set; }
        public string CRE_TIPO_ABONO_EXTRA { get; set; }
        public decimal CRE_VALOR_ABONO_EXTRA { get; set; }
        public decimal CRE_VALOR_DEUDA_REMANENTE { get; set; }
        public decimal CRE_VALOR_CANON_INICIAL { get; set; }
        public decimal CRE_VALOR_OPCION_COMPRA { get; set; }
        public decimal CRE_POR_CANON_INICIAL { get; set; }
        public decimal CRE_POR_OPCION_COMPRA { get; set; }
        public decimal CRE_ACUMULADO_INTERES_MORA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_INTERES_MORA { get; set; }
        public decimal CRE_ACUMULADO_INTERES_CORRIENTE { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE { get; set; }
        public decimal CRE_ACUMULADO_SEGURO_HOGAR { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR { get; set; }
        public decimal CRE_ACUMULADO_SEGURO_VIDA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA { get; set; }
        public decimal CRE_ALIVIO_FRECH { get; set; }
        public int CRE_NUMERO_CUOTAS_CANCELADAS { get; set; }
        public Guid CRE_CREADO_POR { get; set; }
        public DateTime CRE_FECHA_CREACION { get; set; }
        public Guid CRE_MODIFICADO_POR { get; set; }
        public DateTime CRE_FECHA_MODIFICACION { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public int TIPO_IDENTIFICACION { get; set; }
        public int CLI_EDAD { get; set; }
        public string CATEGORIA { get; set; }
        public string GRADO { get; set; }
        public string CRE_DIRECCION_INMUEBLE { get; set; }
        public int CIUDAD_INMUEBLE { get; set; }
        public string NUMERO_MATRICULA { get; set; }
        public string ESCRITURA_INMUEBLE { get; set; }
        public DateTime FECHA_ESCRITURA { get; set; }
        public string NOTARIA { get; set; }
        public string SIJ_LINDEROS { get; set; }
        public string DESCRIPCION_INMUEBLE { get; set; }
        public int SOC_SCORE { get; set; }
        public DateTime FECHA_SOLICITUD { get; set; }
        public DateTime CRE_FECHA_FINALIZA_PAGOS { get; set; }
        public decimal CRE_CANON_EXTRAORDINARIO { get; set; }
        public DateTime CRE_FECHA_CAUSACION { get; set; }
        public decimal CRE_PROVISION { get; set; }
        public decimal CRE_PROVISION_PORCENTAJE { get; set; }
        public string CRE_CALIFICACION { get; set; }
        public string CLASE_INMUEBLE { get; set; }
        public int UEJ_ID { get; set; }
        public int TCR_ID_ESTADO_CREDITO { get; set; }
        public decimal CRE_SALDO_CAPITAL_TRADICIONAL { get; set; }
        public decimal CRE_SALDO_INICIAL_REDUCIDO { get; set; }
        public decimal CRE_VALOR_CUOTA_TRADICIONAL { get; set; }
        public decimal CRE_ACUMULADO_CAPITAL { get; set; }
        public decimal CRE_PROVISION_ADICIONAL { get; set; }
        public int CLC_ID { get; set; }
        public decimal RECURSOS_FUTUROS { get; set; }
        public decimal CRE_PORCENTAJE_SEGURO_VIDA { get; set; }
        public decimal CRE_PORCENTAJE_EXTRA_PRIMA_SEGURO_VIDA { get; set; }

        public string CRE_ASEGURADORA_SOLIDARIA { get; set; }
    }
}
