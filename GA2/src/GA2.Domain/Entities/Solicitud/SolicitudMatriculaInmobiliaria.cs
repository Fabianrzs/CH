using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class SolicitudMatriculaInmobiliaria
    {
        public Guid MAI_ID { get; set; }
        public string MAI_NUMERO_MATRICULA { get; set; }
        public string MAI_DIRECCION { get; set; }
        public DateTime MAI_FECHA_REGISTRO { get; set; }
        public int DPD_ID_FK { get; set; }
        public int DPC_ID_FK { get; set; }
        public bool MAI_PRINCIPAL { get; set; }
        public bool MAI_ESTADO { get; set; }
        public bool MAI_BLOQUEO { get; set; }
        public int OFC_ID { get; set; }
        public string OFC_DESCRIPCION { get; set; }
        public string DPD_OFICINA_REGISTRO { get; set; }
        public int DPD_ID { get; set; }
        public int MAI_TRAMITADA { get; set; }
        public int DPC_ID { get; set; }
        public string DPD_NOMBRE { get; set; }
        public string DPC_NOMBRE { get; set; }
        public int DPD_OFC_ID { get; set; }
        public string OFC_CODIGO { get; set; }
        public string MAI_NUM_MATRICULA { get; set; }

        public string MAI_TIPO_BLOQUEO { get; set; }
        public DateTime MAI_FECHA_BLOQUEO { get; set; }
    }
}
