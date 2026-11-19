using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class SolicitudAdjudicacionDto
    {

        public Guid SIA_ID { get; set; }
        public Guid SOL_ID_FK { get; set; }
        public string SIA_TIPO_VIVIENDA { get; set; }
        public string SIA_DIRECCION { get; set; }
        public string SIA_MATRICULA { get; set; }
        public string SIA_VALOR_SUBSIDIADO { get; set; }
        public string SIA_CIUDAD { get; set; }


    }
}
    