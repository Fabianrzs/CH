using System;

namespace GA2.Application.Dto
{
    /// <summary>
    /// Propiedades Dto de Tipo Actividad
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>10/05/2021</date>
    public class MatriculaCreditoDto
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
        public string AreaConstruida { get; set; }  

    }
}


