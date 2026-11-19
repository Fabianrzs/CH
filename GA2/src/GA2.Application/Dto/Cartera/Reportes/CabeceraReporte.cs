using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class CabeceraReporte
    {
        public string Nombre { get; set; }

        public IEnumerable<CabeceraReporte> Subtitulos { get; set; }

        public double? AnchoCelda { get; set; }

        public CabeceraReporte(string name, double? cellWidth = null, params CabeceraReporte[] sustitulos)
        {
            Nombre = name;
            AnchoCelda = cellWidth;
            Subtitulos = sustitulos;
        }

        public CabeceraReporte(string name, params CabeceraReporte[] sustitulos) : this(name, null, sustitulos)
        {

        }

        public static implicit operator CabeceraReporte(string name)
        {
            return new CabeceraReporte(name);
        }
    }
}