using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.TusDatos.Dto
{
    public class DatosClienteJobResponse
    {
        public string? StatusJobTusDatos { get; set; }

        public HistoryDataCustomer? HistoryDataCustomer { get; set; }
    }
}
