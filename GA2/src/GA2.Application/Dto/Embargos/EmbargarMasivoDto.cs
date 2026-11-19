using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Embargos
{
    public class EmbargarMasivoDto
    {
        public Guid IdCuenta { get; set; }
        public List<EmbargarDTO> ListaMasiva { get; set; }
    }

    public class EmbargarDTO
    {
        public Guid TIE_ID { get; set; }
        public float ECC_VALOR { get; set; }
        public DateTime ECC_FECHA_INICIO_EMBARGO { get; set; }
        public DateTime ECC_FECHA_FIN_EMBARGO { get; set; }
        public Guid TRE_ID { get; set; }
        public bool ECC_INDICADOR_CESANTIAS { get; set; }


        #region "Propiedades x Conceptos"

        public int CTA_ID { get; set; }  // Cuenta
        public int CNC_ID { get; set; } // Concepto
        public int CCT_SALDO { get; set; } // Saldo Concepto
        //public Int32 CCT_SVA { get; set; } // Saldo Disponible Actual

        #endregion
    }
}
