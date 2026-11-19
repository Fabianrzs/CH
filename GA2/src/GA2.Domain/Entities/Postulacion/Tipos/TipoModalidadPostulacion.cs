using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class TipoModalidadPostulacion
    {
        [Key]
        public int MPO_ID { get; set; }
        public string MPO_NOMBRE { get; set; }
        public DateTime MPO_FECHA_INSERCION { get; set; }
        public int MPO_ESTADO { get; set; }
        public int MPO_ID_SOLICITUD { get; set; }
    }
}
