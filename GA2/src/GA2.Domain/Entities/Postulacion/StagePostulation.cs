using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Postulacion
{
    public class StagePostulation
    {
        public string Activity { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
