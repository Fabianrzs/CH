using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class ArchivoConErrorDto
    {
        public int IdDocumento { get; set; }
        public int ConsecutivoEncabezado { get; set; }
        public int ConsecutivoFin { get; set; }
        public int IdTipoArchivo { get; set; }
        public string TipoArchivo { get; set; }
        public string NombreUnidadEjecutora { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public string Periodo { get; set; }
        public string IndicadorControl { get; set; }
        public string SumatoriaAporte { get; set; }
        public int CodigoFinArchivo { get; set; }
        public string TipoError { get; set; }
        public List<ErrorCargueArchivoDto> Errores { get; set; }

        //cuerpo
        public int ConsecutivoCuerpo { get; set; }
        public int digitoFuerza { get; set; }
        public int TipoDocumento { get; set; }
        public string Identificacion { get; set; }
        public string CodigoMilitar { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public int Embargo { get; set; }
        public int BaseCalculo { get; set; }
        public int ValorAporte { get; set; }
        public string PeriodoCuerpo { get; set; }
        public int Grado { get; set; }
        public string UnidadOperativa { get; set; }

    }
}
