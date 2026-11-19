using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class SolicitudTerceroApoderado
    {
        public Guid SOL_ID_FK { get; set; }
        public int? TER_TIPO_TERCERO_FK { get; set; }
        public int TID_ID_FK { get; set; }
        public string TER_NUMERO_DOCUMENTO { get; set; } = ""; 
        public string TER_NOMBRE_RAZON_SOCIAL { get; set; }
        public int? TER_PARENTESCO { get; set; }
        public bool TER_ESAUTORIZADO { get; set; }
        public Guid CREATEDO_POR { get; set; }

        // campos adicionales
        public Guid TER_ID { get; set; }
        public string TER_PRIMER_NOMBRE { get; set; }
        public string TER_SEGUNDO_NOMBRE { get; set; }
        public string TER_PRIMER_APELLIDO { get; set; }
        public string TER_SEGUNDO_APELLIDO { get; set; }
        public DateTime TER_FECHA_REGISTRO { get; set; }
    }


    public class SolicitudBeneficiario
    {
        public Guid SOL_ID_FK { get; set; }
        public int? TER_TIPO_TERCERO_FK { get; set; }
        public int TID_ID_FK { get; set; }
        public string? CVL_DESCRIPCION { get; set; }
        public string? TID_DESCRIPCION { get; set; }
        public string TER_NUMERO_DOCUMENTO { get; set; } = "";
        public int? TER_PARENTESCO { get; set; }
        public Guid CREATEDO_POR { get; set; }
        public decimal? TER_VALOR_RETIRAR { get; set; }
        public Guid TER_ID { get; set; }
        public string TER_PRIMER_NOMBRE { get; set; }
        public int? CVL_ID { get; set; }
        public string TER_SEGUNDO_NOMBRE { get; set; }
        public string TER_PRIMER_APELLIDO { get; set; }
        public string TER_SEGUNDO_APELLIDO { get; set; }
        public DateTime TER_FECHA_REGISTRO { get; set; }
    }
}
