using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class TerceroVendedorRespuestaDto
    {
        public Guid id { get; set; }
        public int tipoDoc { get; set; }
        public LlaveValorDto tipoDocObj { get; set; }
        public string numDoc { get; set; }
        public string razonsocial { get; set; }
        public string direccion { get; set; }
        public LlaveValorDto departamentoVendedor { get; set; }
        public int ciudad { get; set; }
        public LlaveValorDto ciudadObj { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
    }
}
