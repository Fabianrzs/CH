using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.CargueNonima.Document
{
    public class ConsultarDocumentosCarga
    {
        public int DCT_ID { get; set; }
        public int TDC_ID { get; set; }
        public string DCT_NOMBRE { get; set; }
        public long DCT_REGISTROS { get; set; }
        public DateTime DCT_FECHA_INICIAL { get; set; }
        public int ESD_ID { get; set; }
        public int CED_ID { get; set; } //Causal estado documento
        public int UEJ_ID { get; set; } //Unidad ejecutora
        public DateTime DCT_FECHA_FINAL { get; set; }
        public int DCT_ID_ANULA { get; set; }
        public string DCT_ALERTA { get; set; }
        public Guid DCT_CREADOPOR { get; set; }
        public DateTime DCT_FECHACREACION { get; set; }
        public DateTime DCT_FECHAMODIFICACION { get; set; }
        public Guid DCT_MODIFICADOPOR { get; set; }
        public string ESTADO { get; set; }
        public string USUARIO { get; set; }
    }
}
