using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.Proyecto
{
    public class InmublesProyectoDto
    {
        public List<InmueblesDto> inmuebles { get; set; }
        public List<ClaseInmuebleDto> claseInmuebles { get; set; }
        public List<ListaAvanceDto> listaAvances { get; set; }  
    }
    public class InmueblesDto
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
