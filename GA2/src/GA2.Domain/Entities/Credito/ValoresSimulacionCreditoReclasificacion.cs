using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ValoresSimulacionCreditoReclasificacion
    {
        public int VSC_ID { get; set; }

        public string NUMERO_DOCUMENTO { get; set; }

        public decimal TASA_CREDITO { get; set; }
        public decimal TASA_NOMINAL { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public decimal SEGURO_HOGAR { get; set; }
        public decimal SALARIO_MINIMO { get; set; }
        public decimal SEGURO_VIDA_EXTRAPRIMA { get; set; }
        public decimal TASA_MIXTA { get; set; }
        public decimal TASA_MIXTA_EXTRAPRIMA { get; set; }
        public decimal FACTOR { get; set; }
        public decimal FACTOR_EXTRA_PRIMA { get; set; }
        public decimal PLAZO { get; set; }
        public decimal VALOR_PRESTAMO { get; set; }
        public decimal CUOTA_SIN_SEGURO_HOGAR { get; set; }
        public decimal CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA { get; set; }
        public decimal CUOTA_SEGURO_HOGAR { get; set; }
        public decimal CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA { get; set; }
        public decimal VALOR_INMUEBLE { get; set; }
        public decimal VALOR_AVALUO { get; set; }
        public decimal CAPACIDAD_PAGO { get; set; }
        public decimal VALOR_PRESTAMO_SEGUN_CUOTA { get; set; }

        public DateTime FECHA_CREACION { get; set; }
    }
}
