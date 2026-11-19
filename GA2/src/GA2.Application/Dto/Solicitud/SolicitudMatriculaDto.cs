using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class SolicitudMatriculaDto
    {
        public string NumeroMatricula { get; set; }
        public int? Bandera { get; set; }
        public string Direccion { get; set; }
        public int DepartamentoId { get; set; }
        public int CiudadId { get; set; }
        public bool EsPrincipal { get; set; }
        public int? OficinaRegistroId { get; set; } = null; 
        public string? OficinaCodigo { get; set; } = String.Empty;
        public bool ConyugeActivo { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Documento { get; set; }


        //campos adicionales
        public Guid id { get; set; }
    }

}
