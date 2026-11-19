using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class SolicitudMatricula
    {
        public Guid SOL_ID_FK { get; set; }
        public string MAI_NUMERO_MATRICULA { get; set; }
        public DateTime MAI_FECHA_REGISTRO { get; set; }
        public string MAI_DIRECCION { get; set; }
        public int DPD_ID_FK { get; set; }
        public int DPC_ID_FK { get; set; }
        public Guid CREATEDO_POR { get; set; }
        public bool MAI_PRINCIPAL { get; set; }
        public int OFC_ID { get; set; }

        // campos adicionales
        public Guid MAI_ID { get; set; }
        public string OFC_CODIGO { get; set; }
        public string MAI_NUM_MATRICULA { get; set; }
        public DateTime MAI_FECHA_BLOQUEO { get; set; }
        public bool MAI_CONYUGE_ACTIVO { get; set; }
        public string MAI_PRIMER_NOMBRE_CONYUGE { get; set; }
        public string MAI_SEGUNDO_NOMBRE_CONYUGE { get; set; }
        public string MAI_PRIMER_APELLIDO_CONYUGE { get; set; }
        public string MAI_SEGUNDO_APELLIDO_CONYUGE { get; set; }
        public string MAI_CEDULA_CONYUGE { get; set; }        
    }
}
