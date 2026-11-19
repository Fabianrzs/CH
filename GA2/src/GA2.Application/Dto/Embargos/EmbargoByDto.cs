using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class EmbargoByDto
    {
        public EmbargosDto Embargo { get; set; }
        public List<BeneficiariosPagoEmbargoDto> Beneficiarios { get; set; }
        public List<EmbargoCuentaConceptoDto> Conceptos { get; set; }


    }
}
