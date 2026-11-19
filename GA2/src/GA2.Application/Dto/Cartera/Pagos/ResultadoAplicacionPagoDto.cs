using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ResultadoAplicacionPagoDto
    {
		public string Fuerza_Nombre { get; set; }
		public string Tipo_Nómina { get; set; } 
		public string Concepto_Cód { get; set; }
		public string Concepto { get; set; }
		public string Funcionario { get; set; }
		public string Documento { get; set; } 
		public string Código_Militar { get; set; }
		public decimal Valor { get; set; }
		public string Fecha_Desde { get; set; }
		public string Fecha_Hasta { get; set; }
		public Guid CRE_ID { get; set; }
		public decimal Cuota { get; set; }
		public int Días_mora { get; set; }
		public decimal Interes_cte { get; set; }
		public decimal Interes_mora { get; set; }
		public decimal Seguro_vida { get; set; }
		public decimal Seguro_hogar { get; set; }
		public decimal Capital { get; set; }
		public DateTime Fecha_proceso { get; set; }
		public DateTime Fecha_aplicacion { get; set; }
		public string Observaciones { get; set; }
		public string TipoCredito { get; set; }
		public string responseSeven { get; set; }
	}
}
