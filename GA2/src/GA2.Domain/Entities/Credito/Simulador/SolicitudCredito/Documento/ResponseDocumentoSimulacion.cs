using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito.Simulador.SolicitudCredito.Documento
{
    public class ResponseDocumentoSimulacion
    {
        public Guid DSC_ID { get; set; }
        public Guid SLS_ID { get; set; }
        public int TDC_ID { get; set; }
        public string DSC_CODIGO_BARRAS { get; set; }
        public DateTime DSC_FECHA_DOCUMENTO { get; set; }
        public int DSC_NUMERO_FOLIOS { get; set; }
        public long DSC_TAMAÑO { get; set; }
        public string DSC_RUTA_IMAGEN { get; set; }
        public string DSC_ESTADO { get; set; }
        public Guid DSC_CREADO_POR { get; set; }
        public DateTime DSC_FECHA_CREACION { get; set; }
    }
}
