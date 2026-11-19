using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class SimulacionCreditoArchivo
    {
        public double cuotasSectorReal { get; set; }
        public double cuotasSectorFinanciero { get; set; }
        public int riesgoMinimo { get; set; }
        public bool estadoDesicion { get; set; }
        public byte[] ArchivoBytes { get; set; }
        public string FileName { get; set; }
        public string descripcionDecision { get; set; }
        public double totalObligacionMora { get; set; } 
        public string fechaPermanenciaVigente { get; set; } 
        public bool comprarCarteraSinViiviendaValidacion { get; set; }

    }
}
