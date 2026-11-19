using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ArchivoSolicitudDto
    {
        public Guid Id { get; set; }
        public string NombreArchivoCargado { get; set; }
        public string RutaArchivo { get; set; }
        public string Extension { get; set; }
        public string NombreArchivo { get; set; }
        public int Orden { get; set; }
        public string? Observacion { get; set; }
    }
}
