using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class ConsultarArchivosPorEntidadDto
    {
        public Guid IdArchivoParametrizado { get; set; }
        public string NombreArchivo { get; set; }
        public string DescripcionArchivo { get; set; }
        public bool EsRequerido { get; set; }
        public int Orden { get; set; }
        public bool EsMultiple { get; set; }
        public string Formato { get; set; }
        public bool EstadoParametrizacion { get; set; }
        public int TipoPersonaId { get; set; }
        public string TipoPersona { get; set; }
        public Guid IdArchivoCargado { get; set; }
        public string NombreArchivoCargado { get; set; }
        public string RutaStorage { get; set; }
        public string Extension { get; set; }
        public string NumeroFolio { get; set; }
        public DateTime FechaCargue { get; set; }
        public bool NuevoCargue{ get; set; }
    }
}
