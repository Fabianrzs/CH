using System;

namespace GA2.Application.Dto.Proyecto
{
    public class ListaAvanceResponseDto
    {
        public int Codigo { get; set; }
        public Guid ProyectoId { get; set; }
        public int Porcentaje { get; set; }
        public string ItemAvance { get; set; }
    }
}
