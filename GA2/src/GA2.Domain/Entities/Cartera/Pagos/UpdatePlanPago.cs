using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Pagos
{
    public class UpdatePlanPago
    {
        public long numeroCredito { get; set; }
        public DateTime fechaPago { get; set; }
        public int appId { get; set; }
        public Guid cpgId { get; set; }
        public int mcoCont { get; set; }
        public int mcoNume { get; set; }
        public int ccId { get; set; }
    }

    public class UpdatePlanPagoAdelantoCuota
    {
        public long numeroCredito { get; set; }
        public int mcoCont { get; set; }
        public int mcoNume { get; set; }
    }
}
