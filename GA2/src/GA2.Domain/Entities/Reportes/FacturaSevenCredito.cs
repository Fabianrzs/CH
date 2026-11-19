using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Reportes
{
    public class FacturaSevenCredito
    {
        public Guid SOC_ID { get; set; }
        public int FAC_TIPO_DOCUMENTO { get; set; }
        public string FAC_NUMERO_DOCUMENTO { get; set; }
        public string FAC_NOMBRE_AFILIADO { get; set; }
        public string FAC_SEGUNDO_NOMBRE_AFILIADO { get; set; }
        public string FAC_APELLIDO_AFILIADO { get; set; }
        public string FAC_SEGUNDO_APELLIDO_AFILIADO { get; set; }
        public DateTime FAC_FECHA_REGISTRO_TERCERO { get; set; }
        public int FLUJO { get; set; }

        public int FAC_TIPO_DOCUMENTO_VENDEDOR { get; set; }
        public string FAC_NUMERO_DOCUMENTO_VENDEDOR { get; set; }
        public string FAC_NOMBRE_COMPLETO_VENDEDOR { get; set; }
        public int FAC_TIPO_CUENTA_VENDEDOR { get; set; }
        public string FAC_NUMERO_CUENTA_VENDEDOR { get; set; }
        public Guid FAC_BANCO_VENDEDOR { get; set; }
        public DateTime FAC_FECHA_REGISTRO_VENDEDOR { get; set; }

        public int FAC_CDP { get; set; }
        public int FAC_TIPO_CREDITO { get; set; }
        public decimal FAC_VALOR_DESEMBOLSO { get; set; }
        public string FAC_RADICADO { get; set; }
        public string FAC_AREA_NEGOCIO { get; set; }
        public DateTime FAC_FECHA_REGISTRO_CDP { get; set; }


        public string FAC_NUMERO_FACTURA { get; set; }
        public DateTime FAC_FECHA_REGISTRO_FACTURA { get; set; }

    }
}
