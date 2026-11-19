using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class ObtenerFormularioSolicitudDto
    {
        public ObtenerFormularioSolicitudDto() {}

        public int id { get; set; }
        public string titulo { get; set; }
        public string subtitulo { get; set; }
        public string icono { get; set; }
        public bool botonera { get; set; }
        public string formulario { get; set; }
    }

    public class FormulariosSolicitudDto
    {
        public FormulariosSolicitudDto()
        {
            this.formularios = new List<string>();
        }

        public int id { get; set; }
        public string titulo { get; set; }
        public string subtitulo { get; set; }
        public string icono { get; set; }
        public bool botonera { get; set; }
        public List<string> formularios { get; set; }
    }
}

