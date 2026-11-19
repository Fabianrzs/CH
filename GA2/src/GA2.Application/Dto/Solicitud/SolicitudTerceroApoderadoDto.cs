using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class SolicitudTerceroApoderadoDto
    {
        public int? TipoDoc { get; set; }
        public string NumDoc { get; set; }
        public string Razonsocial { get; set; }
        public int? Parentesco { get; set; }
        public bool EsAutorizado { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }

    }
    public class SolicitudBeneficiarioDto
    {
        public Guid? id { get; set; }
        public int? TipoDoc { get; set; }
        public string NumDoc { get; set; }
        public int? Parentesco { get; set; }
        public LlaveValorDto? parentescoObj { get; set; }
        public LlaveValorDto? tipoDocObj { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public decimal? ValorRetirarDos { get; set; }

    }
}
