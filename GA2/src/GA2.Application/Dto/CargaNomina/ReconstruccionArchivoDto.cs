using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.CargaNomina
{
    public class ReconstruccionArchivoDto
    {
        public string NombreArchivo { get; set; }
        public string Header { get; set; }
        public List<string> Body { get; set; }
        public string FinArchivo { get; set; }
    }

    public class ArchivoNombreDto
    {
        public string NAN_NOMBRE_LOGICO { get; set; }
    }

    public class ArchivoHeaderDto
    {
        public string HEADER_RECONSTRUIDO { get; set; }
    }

    public class ArchivoBodyDto
    {
        public string LINEA_BODY { get; set; }
    }

    public class ArchivoFinDto
    {
        public string LINEA_FIN_ARCHIVO { get; set; }
    }

}


