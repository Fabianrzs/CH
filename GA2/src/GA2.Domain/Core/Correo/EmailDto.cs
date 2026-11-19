using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Correo
{
    public class EmailDto
    {
        public List<string> Destinatarios { get; set; }

        public string Asunto { get; set; }

        public string RutaHtml { get; set; }

        public string RutaCss { get; set; }

        public Dictionary<string, string> Parametros { get; set; }
    }
}
