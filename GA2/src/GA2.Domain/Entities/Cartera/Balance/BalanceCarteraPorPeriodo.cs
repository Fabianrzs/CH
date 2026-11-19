using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Balance
{

    public class BalanceCarteraPorPeriodo
    {
        public Guid BLC_ID { get; set; }
        public DateTime BLC_PERIODO { get; set; }
        public string BLC_CUENTAS_NIF { get; set; }
        public string BLC_CUENTA_CONTABLE { get; set; }
        public string BLC_DESCRIPCION_CUENTA { get; set; }
        public decimal BLC_SALDO_INICIAL { get; set; }
        public decimal BLC_SALDO_FINAL { get; set; }
        public decimal BLC_DEBITOS { get; set; }
        public decimal BLC_CREDITOS { get; set; }
    }

}
