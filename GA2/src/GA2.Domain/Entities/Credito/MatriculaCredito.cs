using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class MatriculaCredito
    {
        public Guid IdMatricula { get; set; }
        public string NumeroMatricula { get; set; }
        public DateTime FechaMatricula { get; set; }
        public string Chip { get; set; }
        public string CedulaCatastral { get; set; }
        public decimal AreaPrivadaConstruida { get; set; }
        public decimal AreaAvaluoComercial { get; set; }
        public decimal AreaAvaluoCatastral { get; set; }
        public Guid SocId { get; set; }
        public int Tipo_Matricula { get; set; }
    }
}