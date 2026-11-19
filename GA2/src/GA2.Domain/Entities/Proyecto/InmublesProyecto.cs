using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class InmublesProyecto
    {
        public int CodigoTipoInmueble { get; set; }
        public double Valor { get; set; }
        public double AreaPrivada { get; set; }
        public double AreaConstruida { get; set; }
        public int CodigoTipoVivienda { get; set; }
        public int CodigoClaseVivienda { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public Guid ProyectoId { get; set; }
    }
}
