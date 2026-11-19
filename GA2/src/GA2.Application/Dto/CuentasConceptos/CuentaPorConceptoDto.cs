using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class CuentaPorConceptoDto
    {
        public int IdConceptoCuenta { get; set; }
        public int IdCuenta { get; set; }
        public int IdConcepto { get; set; }
        public string DescripcionConcepto { get; set; }
        public double ConceptoSaldoCuenta { get; set; }
        public DateTime ConceptoFechaUltimoAporte { get; set; }
        public int ConceptoIngreso { get; set; }
        public double ConceptoSva { get; set; }
        public int cuentaNumero { get; set; }
        public DateTime CuentaFechaCreación { get; set; }
        public DateTime CuentaPrimerAporte { get; set; }
        public double CuentaSaldo { get; set; }
        public double CuentaSaldoCanje { get; set; }
        public double CuentaSaldoDisponible { get; set; }
        public int CuentaCuotas { get; set; }
    }
}
