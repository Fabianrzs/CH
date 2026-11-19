using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class DocumentoCargarSolicitudDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Obligatorio { get; set; }
        public string Carga { get; set; }
        public string Visualiza { get; set; }
        public string VisaulizaCliente { get; set; }
        public string CargaPreAprobado { get; set; }
        public bool Cargado { get; set; }
        public string Ruta { get; set; }
    }
}
