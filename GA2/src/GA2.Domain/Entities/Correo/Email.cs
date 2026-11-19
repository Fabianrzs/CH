using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Correo
{
    public class Email
    {
        public List<string> Destinatarios { get; set; }

        public string Asunto { get; set; }

        public string RutaHtml { get; set; }

        public string RutaCss { get; set; }

        public string RutaImagenEncabezado { get; set; }

        public Dictionary<string, string> Parametros { get; set; }

        public string Modulo { get; set; }
        public Guid UsuarioCreacion { get; set; }
        public string IpOrigen { get; set; }
        public List<string>? Adjuntos { get; set; }
        public bool EnvioBase64 { get; set; }
        public string AdjuntoBase64 { get; set; }
        public string NombreDocumentoAdjunto { get; set; }
    }
}
