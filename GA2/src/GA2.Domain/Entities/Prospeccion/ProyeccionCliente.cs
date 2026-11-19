using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ProyeccionCliente
    {
        public int CRG_ID { get; set; }
        public string CRG_DESCRIPCION { get; set; }
        public int CTG_ID { get; set; }
        public double CRG_FACTOR_SUBS { get; set; }
        public int CRG_AÑOS_ASCENSO { get; set; }
        public int CRG_ORDEN { get; set; }
        public int CRG_ESTADO { get; set; }
        public int ID_COLOR { get; set; }
        public decimal CRA_SALARIO_BASICO { get; set; }
        public int CRA_AÑO { get; set; }
        public bool CRG_PROYECTA_SM { get; set; }
        public bool CRG_PROYECTA_SUBSIDIO { get; set; }

    }

    public class SmlvAnual
    {
        public Guid SMLV_ID { get; set; }
        public decimal SMLV_SALARIO { get; set; }
        public int SMLV_ANIO { get; set; }
    }

}
