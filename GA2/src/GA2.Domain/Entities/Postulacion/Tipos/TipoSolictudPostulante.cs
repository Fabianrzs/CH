using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class TipoSolictudPostulante
    {
    [Key]
    public int TSP_ID  { get; set; }
    public string TSP_NOMBRE { get; set; }
    public DateTime  TSP_FECHA_INSERCION { get; set; }
    public int TSP_ESTADO { get; set; }
    }
}
