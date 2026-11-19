using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.TusDatos.Dto
{
    public class DatosClienteResponse
    {
        public string? JobId { get; set; }

        public bool IsFromHistory { get; set; }

        public HistoryDataCustomer? HistoryDataCustomer { get; set; }
    }
    public class HistoryDataCustomer
    {
        public bool IsPEPS { get; set; }

        public string? DetectionRisk { get; set; }
    }
}
