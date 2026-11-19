using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class ListaAvanceResponse
    {
        public int Codigo { get; set; }
        public Guid ProyectoId { get; set; }
        public int Porcentaje { get; set; }
        public string ItemAvance { get; set; }
    }
}
