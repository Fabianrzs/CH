using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class TipoListaArchivo
    {
        public Guid DFS_ID { get; set; }
        public string DFS_MOMBRE { get; set; }
        public string DFS_DESCRIPCION { get; set; }
        public int DFS_REQUERIDO_ { get; set; }
        public int DFS_ORDEN { get; set; }
        public int DFS_ESTADO { get; set; }
        public string DFS_FORMATO { get; set; }
        public Guid Asp_Id { get; set; }
        public int ARCHIVO_CARGADO { get; set; }
        public int NUMERO_FOLIO { get; set; }
    }
}
