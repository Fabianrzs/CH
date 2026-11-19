using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.ValoresCategoria
{
    public class ValoresCategoriaDto
    {
        public int Crg_Id { get; set; }
        public string Crg_Descripcion { get; set; }
        public int Ctg_Id { get; set; }
        public decimal Crg_Factor_Subs { get; set; }
        public int Crg_Anios_Ascenso { get; set; }
        public decimal Cra_Salario_Basico { get; set; }
        public decimal Cra_Salario_Total { get; set; }
        public int Cra_Anio { get; set; }
        public bool Crg_ProyectaSM { get; set; }
        public bool Crg_ProyectaSubsidio { get; set; }
    }

    public class CrearValoresDto
    {
        public int Cra_Anio { get; set; }
        public decimal Porc_Aumento { get; set; }
    }
}
