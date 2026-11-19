using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Prospeccion
{
    public class InformacionCongelamientoRecursos
    {
        public string NumeroDocumento { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public List<CongelamientoRecursosProspeccion> ListaRecursosCongelados { get; set; }
    }
}
