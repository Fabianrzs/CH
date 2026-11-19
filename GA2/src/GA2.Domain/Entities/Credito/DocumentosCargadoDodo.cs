using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class DocumentosCargadoDodo
    {
        public Guid DCD_ID { get; set; }

        public string DCD_DESCRIPCION { get; set; }

        public DateTime DCD_FECHA_CARGUE { get; set; }

        public bool DCD_CARGADO { get; set; }

        public Guid SLS_ID { get; set; }

        public string RUTA { get; set; }

        public int TDC_ID { get; set; }

        public bool DCD_CARGUE_361 { get; set; }

        public bool DCD_CARGUE_83 { get; set; }

        public int DCD_ESTADO { get; set; }

        public Guid? DCD_CREADO_POR { get; set; }

        public DateTime DCD_FECHA_CREACION { get; set; }

        public Guid? DCD_MODIFICADO_POR { get; set; }

        public DateTime? DCD_FECHA_MODIFICACION { get; set; }
    }
}
