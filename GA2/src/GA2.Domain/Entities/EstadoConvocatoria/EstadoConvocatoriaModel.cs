using System;
using System.ComponentModel.DataAnnotations;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Entity de estado convocatoria
    /// <author>Roberto Carlos Orcasita S.</author>
    /// <date>29/03/2023</date>
    /// </summary>
    public class EstadoConvocatoriaModel
    {
        [Key]
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public DateTime FechaInsercion { get; set; }
        public int Estado { get; set; }
    }
}
