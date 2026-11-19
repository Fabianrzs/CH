using System;

namespace GA2.Application.Dto
{

    /// <summary>
    /// Propiedades Dto de Flujo 
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>24/04/2021</date>
    public class ProyeccionDto
    {
        public int Año { get; set; }
        public string Grado { get; set; }
        public decimal Sueldo { get; set; }
        public decimal Ahorros { get; set; }
        public decimal Int_Ahorros { get; set; }
        public decimal Cesantías { get; set; }
        public decimal Int_Cesantías { get; set; }
    }
}

