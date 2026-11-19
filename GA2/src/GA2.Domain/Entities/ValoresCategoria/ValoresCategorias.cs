using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.ValoresCategoria
{
    public class ValoresCategorias
    {
        public int CRG_ID { get; set; }
        public string CRG_DESCRIPCION { get; set; }
        public int CTG_ID { get; set; }
        public decimal CRG_FACTOR_SUBS { get; set; }
        public int CRG_ANIOS_ASCENSO { get; set; }
        public decimal CRA_SALARIO_BASICO { get; set; }
        public decimal CRA_SALARIO_TOTAL { get; set; }
        public int CRA_ANIO { get; set; }
        public bool CRG_PROYECTA_SM { get; set; }
        public bool CRG_PROYECTA_SUBSIDIO { get; set; }
    }
}
