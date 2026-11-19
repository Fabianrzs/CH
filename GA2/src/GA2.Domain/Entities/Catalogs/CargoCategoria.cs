using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class CargoCategoria
    {
        public int CRG_ID { get; set; }
        public string CRG_DESCRIPCION { get; set; }
        public int CTG_ID { get; set; }
        public double CRG_FACTOR_SUBS { get; set; }
        public int CRG_AÑOS_ASCENSO { get; set; }
        public int CRG_ORDEN { get; set; }
        public int CRG_ESTADO { get; set; }
        public Guid CRG_CREADO_POR { get; set; }
        public DateTime CRG_FECHA_CREACION { get; set; }
        public Guid CRG_MODIFICADO_POR { get; set; }
        public DateTime CRG_FECHA_MODIFICACION { get; set; }
    }
}
