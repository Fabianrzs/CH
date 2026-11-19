using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito.Simulador.DocumentosAct
{
    public class DocumentosActSimulacion
    {
        [Key]
        public Guid SLS_ID { get; set; }
        public int AFL_ID { get; set; }
    }
}
