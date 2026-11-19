using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class CierreMensualCuentas
    {
        public List<int> ListConceptosAhorros { get; set; }
        public int CuentaAbonoAhorros { get; set; }
        public float IPC { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public List<int> ListConceptosCesantias { get; set; }
        public int CuentaAbonoCesantias { get; set; }
        public bool GeneraActualizacion { get; set; }
        public int TipoProceso { get; set; }

    }

    public class ParametrosCierreMensual
    {
        public Guid Usuario { get; set; }
        public bool GeneraActualizacion { get; set; }
        public int TipoProceso { get; set; }
    }

    public class IPC
    {
        public int ID { get; set; }
        public int ANIO { get; set; }
        public int MES { get; set; }
        public float VALOR_IPC { get; set; }
        public float PORCENTAJE_IPC { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
        public int Desde_Anio { get; set; }
        public int Desde_Mes { get; set; }
        public int Hasta_Anio { get; set; }
        public int Hasta_Mes { get; set; }


    }
}
