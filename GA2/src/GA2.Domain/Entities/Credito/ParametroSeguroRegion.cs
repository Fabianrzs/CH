using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ParametroSeguroRegion
    {
        public int REG_ID { get; set; }
        public string REG_NOMBRE { get; set; }
        public double REG_SEGURO { get; set; }
    }

    public class ParametroSeguroRegionGui
    {
        public int REG_ID { get; set; }
        public double REG_SEGURO { get; set; }
    }

}
