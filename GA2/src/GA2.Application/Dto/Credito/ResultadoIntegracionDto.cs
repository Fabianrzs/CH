using System;

namespace GA2.Application.Dto
{

    /// <summary>
    /// Propiedades Dto de Flujo 
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>24/04/2021</date>
    public class ResultadoIntegracionDto
    {
        //public int Id { get; set; }
        public string TipoId { get; set; }
        public string Identificacion { get; set; }
        public Guid ParamId { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public string Resultado { get; set; }
        public Guid CreadoPor { get; set; }

    }
}
