using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Seven
{

    public class RequestCarteraSeven
    {
        public bool AplicaSimulacion { get; set; }
        public long numeroCredito { get; set; }
        public int tipoIdentificacion { get; set; }
        public string NumeroDocumentoCliente { get; set; }
        public Guid cpgId { get; set; }
        public string codigo { get; set; }
        public int nCuota { get; set; }
        public int tipoCredito { get; set; }
        public DateTime FechaPago { get; set; }


    }
}
