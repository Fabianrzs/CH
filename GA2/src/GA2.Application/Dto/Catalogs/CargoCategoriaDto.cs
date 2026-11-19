using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class CargoCategoriaDto
    {
        public int idCargoCategoria { get; set;}
        public string Descripcion { get; set; }
        public int IdCategoria { get; set;}
        public double FactorSubsidio { get; set;}
        public int AniosAscenso { get; set;}
        public int Orden { get; set;}
        public int Estado { get; set;}
        public Guid CreadoPor { get; set;}
        public DateTime FechaCreacion { get; set;}
        public Guid ModificadoPor { get; set;}
        public DateTime FechaModificacion { get; set;}
    }
}
