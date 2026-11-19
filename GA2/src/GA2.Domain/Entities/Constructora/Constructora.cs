using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Constructora
{
    public class Constructora
    {
        public Guid Id_Constructuora { get; set; }
        public string NombreConstructora { get; set; }
        public string Direccion { get; set; }
        public int IdDepartamento { get; set; }
        public int IdCiudad { get; set; }
        public string Email { get; set; }
        public double Nit { get; set; }
        public double Telefono { get; set; }
    }
}
