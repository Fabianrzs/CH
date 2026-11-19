using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class PolizaProyectoResponse
    {
        public int ConceptoId { get; set; }
        public string RadicadoPoliza { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string NumeroPoliza { get; set; }
        public string Aseguradora { get; set; }
        public double ValorPolizas { get; set; }
        public int Porcentaje { get; set; }
        public string Ajustador { get; set; }
        public string Descripcion { get; set; }
        public Guid ProyectoId { get; set; }
    }
}
