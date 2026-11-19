using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.InfoVendedor
{
    public class InformacionVendedor
    {

            public int IVEN_ID { get; set; }
            public Guid SOC_ID { get; set; }
            public int IVEN_TIPO_DOC { get; set; }
            public string IVEN_TIPO_DOC_DESCRIPCION { get; set; }
            public string IVEN_NRO_DOC { get; set; }
            public string IVEN_NOMBRE { get; set; }
            public string IVEN_DIRECCION { get; set; }
            public string IVEN_DEPARTAMANTO { get; set; }
            public string IVEN_CIUDAD { get; set; }
            public string IVEN_CORREO { get; set; }
            public string IVEN_TELEFONO { get; set; }
            public DateTime? IVEN_FECHA_ENTREGA { get; set; }

            public int? IVEN_TIPO_CUENTA { get; set; }
            public Guid? IVEN_BANCO { get; set; }
            public long? IVEN_NRO_CUENTA { get; set; }
            public int? PVD_CODD { get; set; }
            public string ErrorMessage { get; set; }
            public string CiudadDescripcionVendedor { get; set; }
            public string DepartamentoDescripcionVendedor { get; set; }
            public string BANCO_VENDEDOR { get; set; }
            public string TIPO_CUENTA_VENDEDOR { get; set; }

    }

}

