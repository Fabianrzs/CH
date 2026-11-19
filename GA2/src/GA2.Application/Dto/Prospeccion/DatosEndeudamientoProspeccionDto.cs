using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Prospeccion
{
    public class DatosEndeudamientoProspeccionDto
    {
        public Guid EDP_ID { get; set; }
        public string NumeroDocumento { get; set; }
        public decimal SalarioUltimoMes { get; set; }
        public decimal Descuentos { get; set; }
        public decimal SalarioNeto { get; set; }
        public decimal CapacidadPago { get; set; }
        public decimal CanonMensual { get; set; }
        public decimal SectorFinanciero { get; set; }
        public decimal SectorReal { get; set; }
        public int Score { get; set; }
        public decimal DisponibleLibranza { get; set; }
        public decimal EndeudamientoGlobalCuota { get; set; }
        public string ControlNivelEndeudamientoGlobalCuota { get; set; }
        public decimal EndeudamientoGlobalSinCuota { get; set; }
        public string ControlNivelEndeudamientoGlobalSinCuota { get; set; }
        public string PorcentajeEndeudamientoGlobalCuota { get; set; }
        public string PorcentajeEndeudamientoGlobalSinCuota { get; set; }
        public decimal TotalCanonDeducciones { get; set; }
        public string DecisionHomologada { get; set; }

    }
}
