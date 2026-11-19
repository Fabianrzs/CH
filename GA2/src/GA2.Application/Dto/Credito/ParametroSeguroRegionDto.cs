using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class ParametroSeguroRegionDto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public double seguro { get; set; }

    }

    public class ParametroSeguroRegionGuiDto
    {
        public int id { get; set; }
        public double seguro { get; set; }

    }
}
