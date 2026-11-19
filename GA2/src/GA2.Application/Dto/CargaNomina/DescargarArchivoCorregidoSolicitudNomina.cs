using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.CargaNomina
{
    public class DescargarArchivoCorregidoSolicitudNomina
    {

        public EncabezadoArchivosNomina EncabezadoArchivosNomina { get; set; }
        public IEnumerable<CuerpoArchivosNomina> CuerpoArchivosNomina { get; set; }
        public FinArchivosNomina FinArchivosNomina { get; set; }
    }
}
