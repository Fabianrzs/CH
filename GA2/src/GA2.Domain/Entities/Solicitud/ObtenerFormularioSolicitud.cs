using System;

namespace GA2.Domain.Entities
{
    public class ObtenerFormularioSolicitud
    {
        public int ID { get; set; }
        public string TITULO { get; set; }
        public string SUBTITULO { get; set; }
        public string ICONO { get; set; }
        public bool BOTONERA { get; set; }
        public string FORMULARIO { get; set; }
    }
}
