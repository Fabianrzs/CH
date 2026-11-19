using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.BUA
{
    public class CuentaConceptoDto
    {

        public int CuentaId{ get; set; }
        public int ConceptoId { get; set; }
        public double ValorSueldo { get; set; }
        public string TipoValor { get; set; }
        public int IdAfiliado { get; set; }
        public int TipoReintegro { get; set; }
    }
    public class ConceptoUpdate
    {
        public int CuentaId { get; set; }
        public int ConceptoId { get; set; }
        public double Saldo { get; set; }
        public double ValorDevolver { get; set; }
    }
}
