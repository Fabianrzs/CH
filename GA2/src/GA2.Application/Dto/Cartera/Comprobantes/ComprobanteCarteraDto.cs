using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Comprobantes
{
    public class ComprobanteCarteraDto
    {
        public int Comprobante_Id { get; set; }
        public int? Comprobante_Seven_Id { get; set; }
        public DateTime Fecha_Comprobante { get; set; }
        public int Tipo_Comprobante { get; set; }
        public int Departamento { get; set; }
        public int Ciudad { get; set; }
        public int Concepto { get; set; }
        public int Beneficiario { get; set; }
        public int Tipo_Identificacion { get; set; }
        public string Numero_Identificacion { get; set; }
        public decimal Total_Comprobante_Credito { get; set; }
        public decimal Total_Comprobante_Debito { get; set; }
        public bool estado { get; set; }
        public int Estado_Id { get; set; }

        public IEnumerable<MovimientoComprobanteDto> LstMovimientoComprobante { get; set; } = new List<MovimientoComprobanteDto>();
    }
}
