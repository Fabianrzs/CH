using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class ReporteSimulacion60PorCientoDto
    {
        public string salarioTotal { get; set; }
        public string deducciones { get; set; }
        public string salarioNeto { get; set; }
        public string capacidadPago { get; set; }
        public string canonCuotaMensual { get; set; }
        public string nuevoCanonCuotaMensual { get; set; }
        public string disponibleLibranza { get; set; }
        public string controlCapacidadPago { get; set; }
        public string cuotaSectorFinanSoliAse { get; set; }
        public string cuotaSectorReal { get; set; }
        public string totalObliAntesCredito { get; set; }
        public string totalObliNuevoCredito { get; set; }
        public string totalObliNuevoCreditoAjustado{ get; set; }
        public string EndeudaGlobalCanon { get; set; }
        public string EndeudaGlobalSinCanon { get; set; }
        public string EndeudaGlobalNuevoCanon { get; set; }
        public string controlNivelEndeudamiento { get; set; }
        public string NumeroDocumento { get; set; }

    }
}
