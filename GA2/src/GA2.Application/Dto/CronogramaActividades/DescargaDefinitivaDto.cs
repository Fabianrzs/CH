using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.CronogramaActividades
{
    public class DescargaDefinitivaDto
    {
        public int DescargaArchivoId { get; set; }
        public string NombreArchivo { get; set; }
        public int UnidadEjecutoraId { get; set; }
        public string NombreUnidadEjecutora { get; set; }
        public int ConceptoNovedadId { get; set; }
        public string NombreConceptoNovedad { get; set; }
        public DateTime Fecha { get; set; }
        public string Periodo { get; set; }
        public string Estado { get; set; }
        public Guid Usuario { get; set; }
        public string SiglasUnidadEjecutora { get; set; }
        public string CodigoConcepto { get; set; }
        public bool Descargado { get; set; }
    }
}
