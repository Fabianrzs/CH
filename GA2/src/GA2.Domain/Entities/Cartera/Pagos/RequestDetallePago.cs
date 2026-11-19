using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Pagos
{
    public class RequestDetallePago
    {
        public Guid Cpg_id { get; set; }
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public DateTime Mco_fech { get; set; }
        public int Mod_codi { get; set; }
        public string Mco_desc { get; set; }
        public string Cue_codi { get; set; }
        public string Dmc_desc { get; set; }
        public double Dmc_cant { get; set; }
        public string Dmc_refe { get; set; }
        public decimal Dmc_vadb { get; set; }
        public decimal Dmc_vacr { get; set; }
        public string Ter_coda { get; set; }
        public string Arb_codc { get; set; }
        public string Arb_coda { get; set; }
        public string Arb_codp { get; set; }
        public string Arb_cods { get; set; }
        public string Mov_Contable_Seven { get; set; }
        public string Respuesta_Seven { get; set; }
        public DateTime Fecha_Ejecucion { get; set; }
        public int? Cc_Id { get; set; }

    }
    public class RequestCierreMensual
    {
        public int Id { get; set; }
        public int McoContLeasing { get; set; }
        public int McoNumeLeasing { get; set; }
        public int McoContHipotecario { get; set; }
        public int McoNumeHipotecario { get; set; }
    }

}
