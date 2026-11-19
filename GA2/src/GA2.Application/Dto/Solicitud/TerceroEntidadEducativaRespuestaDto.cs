using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class TerceroEntidadEducativaRespuestaDto
    {
        public string razonSocial { get; set; }
        public Guid ID { get; set; }
        public Guid idEntidad { get; set; }
        public Guid? idNivel { get; set; }
        public string reciboPago { get; set; }
        public string fechalimite { get; set; }
        public Guid? idProgramaEntidad { get; set; }

        public int Departamento { get; set; }
        public int Ciudad { get; set; }
        public int Modalidad_estudio { get; set; }
        public int periodoAcademico { get; set; }
        public string Horas { get; set; }
        public string NivelEducativo { get; set; }
        public string ProgramaEducativo { get; set; }
        public DateTime Fecha_de_inicio { get; set; }
        public DateTime Fecha_fin { get; set; }
        public int Acreditada { get; set; }
        public decimal valorRecibo { get; set; }
    }
}
