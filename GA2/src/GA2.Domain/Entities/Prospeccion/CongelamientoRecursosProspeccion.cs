using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Prospeccion
{
    public class CongelamientoRecursosProspeccion
    {
			public Guid CGR_ID { get; set; }
			public string CGR_DESCRIPCION { get; set; }
			public decimal CGR_VALOR { get; set; }
			public Guid CGR_SIMULACION_ID { get; set; }
			public string CGR_NUMERO_DOCUMENTO { get; set; }
			public bool CGR_ESTADO { get; set; }
			public int CGR_ORDEN { get; set; }	
			public DateTime CGR_FECHA_REGISTRO { get; set; }
			public string CGR_TIPO_CREDITO { get; set; }
    }
}
