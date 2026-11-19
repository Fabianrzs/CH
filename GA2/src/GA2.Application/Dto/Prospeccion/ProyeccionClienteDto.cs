using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ProyeccionClienteDto
    {
        public int Id{ get; set; }
        public string Descripcion { get; set; }
        public int categoriaId { get; set; }
        public double factorSubsidio { get; set; }
        public int AñoAscenso { get; set; }
        public int orden { get; set; }
        public int estado { get; set; }
        public int color { get; set; }
        public decimal salariobasico { get; set; }
        public int añosalariobasico { get; set; }
        public bool ProyectaSM { get; set; }
        public bool ProyectaSubsidio { get; set; }
    }

    public class SmlvAnualDto
    {
        public Guid SMLV_ID { get; set; }
        public decimal SMLV_SALARIO { get; set; }
        public int SMLV_ANIO { get; set; }
        public int CTG_ID { get; set; }
        public double VLE_FACTOR_SUBS { get; set; }
    }


    public class ObtenerSmlvAnualDto
    {
        public int CTG_ID { get; set; }
        public int SMLV_ANIO { get; set; }
    }
}
