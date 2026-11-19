using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    /// <summary>
    /// Consulta del detalle de aprobados no desembolsados 
    /// </summary>
    public class ConsultarDetalleAprobadosNoDesembolsadosDTO
    {
        public int top_codi { get; set; }
        public string cue_codi { get; set; }
        public DateTime Mco_fech { get; set; }
        public string ter_coda { get; set; }
        public decimal dmc_vadb { get; set; }
        public decimal dmc_vacr { get; set; }
        public string dmc_desc { get; set; }
        public string tcr_descripcion { get; set; }
        public DateTime fecha_ejecucion { get; set; }
    }
}



  
