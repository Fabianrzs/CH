using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class BeneficiarioEntidadEducativaDto
    {
        public Guid ben_id { get; set; }
        public int tipoDucumentoEntidad { get; set; }
        public string numeroDocumentoInstituto { get; set; }
        public string nombreInstituto { get; set; }
        public string direccionEntidad { get; set; }
        public int departamentoIdEntidad { get; set; }
        public int ciudadIdEntidad { get; set; }
        public string correoEntidad { get; set; }
        public string telefonoEntidad { get; set; }
        public int tipoCuentaEntidad { get; set; }
        public string numeroCuentaEntidad { get; set; }
        public Guid bancoEntidad { get; set; }
        public DateTime fechaLimite { get; set; }
        public Guid SLS_ID { get; set; }
        public string NumeroTitularAfiliado { get; set; }
    }
}
