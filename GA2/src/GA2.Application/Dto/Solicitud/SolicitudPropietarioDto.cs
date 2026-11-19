using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class SolicitudPropietarioDto
    {
        public List<TipoMatricula> Matricula { get; set; }
        public string? NumeroIdentificacion { get; set; }
        public TipoIdentificacionMatricula? TipoIdentificacion { get; set; } 
        public string? RazonSocial { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public DateTime fechaCreacion { get; set; }

    }

    public class TipoIdentificacionMatricula
    {
        public int Id { get; set; } = 1;
        public string Descripcion { get; set; } = "CÉDULA DE CIUDADANÍA";
    }

    public class TipoMatricula
    {
        public string Id { get; set; }
        public string NumeroMatricula { get; set; }
        public string Estado { get; set; }
    }
}
