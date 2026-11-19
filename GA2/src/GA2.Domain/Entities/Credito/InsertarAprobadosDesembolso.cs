using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class InsertarAprobadosDesembolso
    {
        public Guid Soc_id { get; set; }
        public string Identificacion { get; set; }
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public int Mco_nume { get; set; }
        public int Mco_cont { get; set; }

        public DateTime Mco_fech { get; set; }
        public int Mod_codi { get; set; }
        public string Arb_csuc { get; set; }
        public string Mco_desc { get; set; }
        public string Cue_codi { get; set; }
        public string Dmc_desc { get; set; }
        public double Dmc_cant { get; set; }
        public double Dmc_acti { get; set; }
        public string Dmc_refe { get; set; }
        public decimal Dmc_vadb { get; set; }
        public decimal Dmc_vacr { get; set; }
        public decimal Dmc_vaba { get; set; }
        public string Ter_coda { get; set; }
        public string Arb_codc { get; set; }
        public string Arb_coda { get; set; }
        public string Arb_codp { get; set; }
        public string Arb_cods { get; set; }
        public string Ter_codm { get; set; }
        public DateTime Fecha_creacion { get; set; }
    }
}
