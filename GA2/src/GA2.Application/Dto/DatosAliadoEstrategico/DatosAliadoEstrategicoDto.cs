using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.DatosAliadoEstrategico
{
    public class DatosAliadoEstrategicoDto
    {
        public int Dat_Id { get; set; }
        public Guid Soc_Id { get; set; }
        public Guid Nombre_Aliado { get; set; }
        public DateTime Fecha_Pago_Estudio_Titulo { get; set; }
        public DateTime Fecha_Recepcion_Estudio_Titulo { get; set; }
        public string Concepto { get; set; }
        public string Tipo_Parqueadero { get; set; }
        public decimal percentageExtraBonus { get; set; }
        public string talla { get; set; }
        public string peso { get; set; }
        public string dealInsurance { get; set; }
        public string dealInsuranceHome { get; set; }
        public string imc { get; set; }
        public int insuranceID { get; set; }
        public string homeInsuranceID { get; set; }
    }
}