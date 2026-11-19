using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ReporteFuerza
    {
        public string FRZ_ID { get; set; }
        public string ECL_ID { get; set; }
        public int CLI_CONTEO_ESTADO { get; set; }
        public int? TOTAL { get; set; }
    }
}
