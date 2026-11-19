using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class SolicitudTerceroEntidadEducativaDto
    {
        public string Nit { get; set; }
        public string RazonSocial { get; set; }
        public Guid IdEntidad { get; set; }
        public Guid IdNivel { get; set; }
        public Guid? ID { get; set; }
        public string ReciboPago { get; set; }
        public DateTime Fechalimite { get; set; }
         
        public int Departamento { get; set; }
        public int Ciudad { get; set; }
        public int Modalidad_estudio { get; set; }
        public int Periodo_académico { get; set; }
        public string Horas { get; set; }
        public DateTime Fecha_de_inicio { get; set; }
        public DateTime Fecha_fin { get; set; }
        public int Acreditada { get; set; }
        public decimal valorRecibo { get; set; }

    }
}
