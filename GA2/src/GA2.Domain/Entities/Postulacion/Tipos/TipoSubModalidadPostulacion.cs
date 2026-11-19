using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class TipoSubModalidadPostulacion
    {
        [Key]
        public int SPO_ID { get; set; }
        public string SPO_NOMBRE { get; set; }
        public DateTime SPO_FECHA_INSERCION { get; set; }
        public int SPO_ESTADO { get; set; }
        public int SPO_ID_MODALIDAD { get; set; }
    }
}
