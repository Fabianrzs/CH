using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante
{
    public class MovimientoComprobanteDto
    {
        public int Movimiento_Comprobante_CarteraId { get; set; }
        public int Comprobante_CarteraId { get; set; }
        public int Cuenta_Contable { get; set; }
        public string Descripcion_Cuenta { get; set; }
        public decimal Centro_Costo { get; set; }
        public decimal Detalle { get; set; }
        public string Tipo { get; set; }
        public decimal Monto { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
    }
    public class MovimientosComprobanteRequest
    {
        public string Tipo_Credito { get; set; }
        public int Comprobante_Id { get; set; }
        public string CodigoProceso { get; set; }
        public IEnumerable<MovimientosComprobanteDto> Movimientos { get; set; }
    }
       public class MovimientosComprobanteDto
    {
        public int Cuenta_ContableId { get; set; }
        public int Cuenta_Contable { get; set; }
        public decimal? Debito { get; set; }
        public decimal? Credito { get; set; }
        public bool editado { get; set; }
        public bool agregado { get; set; }
    }
}
