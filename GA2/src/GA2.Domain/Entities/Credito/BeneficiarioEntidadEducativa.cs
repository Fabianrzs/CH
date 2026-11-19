using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class BeneficiarioEntidadEducativa
    {
        public Guid BEN_ID { get; set; }
        public int BEN_TIPO_DOCUENTO { get; set; }
        public string BEN_NIT { get; set; }
        public string BEN_RAZON_SOCIAL_ENTIDAD { get; set; }       
        public string BEN_DIRECCION { get; set; }
        public int BEN_DEPARTAMENTO { get; set; }
        public int BEN_CIUDAD { get; set; }
        public string BEN_CORREO { get; set; }
        public string BEN_TELEFONO { get; set; }
        public int BEN_TIPO_CUENTA { get; set; }
        public string BEN_NUMERO_CUENTA { get; set; }
        public Guid BEN_BANCO { get; set; }
        public DateTime BEN_FECHA_LIMITE { get; set; }
        public Guid SLS_ID { get; set; }
        public string DPE_NUMERO_TITULAR { get; set; }
}
}
