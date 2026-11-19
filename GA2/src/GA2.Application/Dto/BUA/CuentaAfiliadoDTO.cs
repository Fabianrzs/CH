using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class CuentaAfiliadoDTO
    {
        public int NumeroCuenta { get; set; }
        public int IdConcepto { get; set; }
        public string Descripcion { get; set; }
        public decimal Saldo { get; set; }
        public int Movimientos { get; set; }
        public decimal ValorSolicitado { get; set; }

    }
}
