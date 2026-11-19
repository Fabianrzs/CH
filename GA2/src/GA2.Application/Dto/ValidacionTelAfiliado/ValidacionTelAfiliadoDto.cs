using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.ValidacionTelAfiliado
{
    public class ValidacionTelAfiliadoDto
    {
        public int Val_Id { get; set; }
        public Guid Soc_Id { get; set; }
        public decimal Conf_Valor_Inmueble { get; set; }
        public int Anio_Fuerzas { get; set; }
        public string Lugar_Trabajo { get; set; }
        public string Cargo_Actual { get; set; }
        public decimal Valor_Arriendo { get; set; }
        public decimal Valor_Activos { get; set; }
        public decimal Valor_Pasivos { get; set; }
        public decimal Monto_Recursos_Propios { get; set; }
        public string Observaciones { get; set; }
    }
}