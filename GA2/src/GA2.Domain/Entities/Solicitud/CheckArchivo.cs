
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class CheckArchivo
    {
        public Guid ASC_ID { get; set; }
        public string ASC_NOMBRE_ARCHIVO { get; set; }
        public DateTime ASC_FECHA_CHECK { get; set; }
        public Guid SOL_ID_FK { get; set; }
    }
}
