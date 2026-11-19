using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ConceptoCuenta
    {

        public int CCT_ID { get; set; }
        public int CTA_ID { get; set; }
        public int CNC_ID { get; set; }
        public double CCT_SALDO { get; set; }
        public DateTime CCT_FECHA_ULTIMO_APORTE { get; set; }
        public int CCT_INGRESOS { get; set; }
        public double CCT_SVA { get; set; }
        public decimal CCT_VALOR_BLOQUEO_EMBARGO { get; set; }
        public  double CCT_VALOR_DEVOLVER { get; set; }
        public double CT_VALOR_DISPONIBLE { get; set; }
    }
    public class LogConceptoCuenta
    {
        public int Id { get; set; }
        public int ConceptoId { get; set; }
        public int CuentaId { get; set; }
        public double ValorBloque { get; set; }
        public DateTime DateCreation { get; set; }
        public double ValorSaldo { get; set; }
        public int IdTipoReintegro { get; set; }
    }
    public class MovimientoReintro
    {
        public int ConceptoId { get; set; }
        public int CuentaId { get; set; }
        public int MovimientoId { get; set; }
        public int DocumentoId { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public double Valor { get; set; }

    }
}
