using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Domain.Entities.Cartera.ProductoPorCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Pagos
{
    public class RequestPagoAbonoCapital
    {
        public List<InsertPlanPagosProductoCartera> LstPln { get; set; }
        public long NumeroCredito { get; set; }
        public string Identificacion { get; set; }
        public DateTime Fecha_Pago_Cuota { get; set; }
        public DateTime Fecha_Pago { get; set; }
        public double NuevaCuotaReducida { get; set; }
        public double NuevaCuotaTradicional { get; set; }
        public double NuevoSaldoReducido { get; set; }
        public double NuevoSaldoTradicional { get; set; }
        public int PlazoTotal { get; set; }
        public int PlazoActual { get; set; }
        public double PagoExtraOrdinario { get; set; }
        public DateTime? FechaFinalizaPagos { get; set; }
    }
}
