using GA2.Domain.Entities.Credito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Prospeccion
{
    public class InformacionConceptosCongelados
    {
        public string NumeroDocumento { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public IEnumerable<ConceptosCongeladosProspeccion> ListaRecursosCongelados { get; set; }
        public List<ConceptosCongeladosProspeccion> ListaRecursosCongelar { get; set; }
    }
}
