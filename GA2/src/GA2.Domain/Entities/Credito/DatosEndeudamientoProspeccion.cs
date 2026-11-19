using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class DatosEndeudamientoProspeccion
    {
        public Guid EDP_ID { get; set; }
        public string EDP_NUMERO_DOCUMENTO { get; set; }
        public decimal EDP_SALARIO_TOTAL { get; set; }
        public decimal EDP_DEDUCCIONES { get; set; }
        public decimal EDP_SALARIO_NETO { get; set; }
        public decimal EDP_CAPACIDAD_PAGO { get; set; }
        public decimal EDP_CANON_MENSUAL { get; set; }
        public decimal EDP_SECTOR_FINANCIERO { get; set; }
        public decimal EDP_SECTOR_REAL { get; set; }
        public int EDP_SCORE { get; set; }
        public decimal EDP_DISPONIBLE_LIBRANZA { get; set; }
        public decimal EDP_ENDEUDAMIENTO_GLOBAL_CUOTA_PORC { get; set; }
        public string EDP_CONTROL_NIVEL_ENDUEDAMIENTO_CUOTA_PORC { get; set; }
        public decimal EDP_ENDEUDAMIENTO_GLOBAL_SIN_CUOTA_PORC { get; set; }
        public string EDP_CONTROL_NIVEL_ENDUEDAMIENTO_SIN_CUOTA_PORC { get; set; }
        public string EDP_ENDUEDAMIENTO_ACTUAL_CUOTA_PORC { get; set; }
        public string EDP_ENDUEDAMIENTO_ACTUAL_SIN_CUOTA_PORC { get; set; }
        public decimal EDP_TOTAL_CANON_DEDUCCIONES { get; set; }
        public string EDP_DECISION_HOMOLOGADA { get; set; }
    }
}
