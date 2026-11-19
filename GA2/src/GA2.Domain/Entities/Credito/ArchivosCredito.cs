using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
	public class ArchivosCredito
	{
		public Guid ATC_ID { get; set; }
		public string ATC_NOMBRE_ARCHIVO { get; set; }
		public bool ATC_REQUERIDO { get; set; }
		public int ATC_ESTADO { get; set; }
		public int ATC_ORDEN { get; set; }
		public string ATC_FORMATO { get; set; }
		public Guid TCR_ID_FK { get; set; }
		public DateTime ATC_FECHA_REGISTRO { get; set; }
    }
}
