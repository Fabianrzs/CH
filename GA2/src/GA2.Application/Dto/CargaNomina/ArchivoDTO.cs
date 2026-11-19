using System;
using System.Collections.Generic;
using System.IO;

namespace GA2.Application.Dto
{
    public class ArchivoDto
    {
        public int IdDocumento { get; set; }
        public int? EsdId { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public int IdTipoArchivo { get; set; }
        public string TipoArchivo { get; set; }
        public string ClasificacionArchivo { get; set; }
        public string Monto { get; set; }
        public int Periodo { get; set; }
        public string FechaCarga { get; set; }
        public string DocumentoNombre { get; set; }
        public long Registros { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public int TotalEstadoError { get; set; }
        public List<ErrorCargueArchivoDto> Errores { get; set; }
        public byte[] File { get; set; }
    }
}
