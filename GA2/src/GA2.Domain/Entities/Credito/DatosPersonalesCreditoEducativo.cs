using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class DatosPersonalesCreditoEducativo
    {
        public Guid DPE_ID { get; set; }
        public int DPE_TIPO_DOCUMENTO { get; set; }
        public string DPE_NUMERO_DOCUMENTO { get; set; }
        public string DPE_PRIMER_NOMBRE_ESTUDIANTE { get; set; }
        public string DPE_SEGUNDO_NOMBRE_ESTUDIANTE { get; set; }
        public string DPE_PRIMER_APELLIDO_ESTUDIANTE { get; set; }
        public string DPE_SEGUNDO_APELLIDO_ESTUDIANTE { get; set; }
        public string DPE_DIRECCION_ESTUDIANTE { get; set; }
        public decimal DPE_VALOR_CREDITO { get; set; }
        public string DPE_PERIODO_LECTIVO { get; set; }
        public Guid FK_ENE_ID { get; set; }
        public int ENE_DPD_ID { get; set; }
        public int ENE_DPC_ID { get; set; }
        public Guid SLS_ID { get; set; }
        public string DPE_NUMERO_IDENTIFICACION_TITULAR { get; set; }
        public string NOMBRE_ENTIDAD_EDUCATIVA { get; set; }
        public int TIPO_DOCUMENTO_ENTIDAD_EDUCATIVA { get; set; }
    }
}
