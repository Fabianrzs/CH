using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.ProductoPorCliente
{
    public class RequestActualizacionProducto
    {
        public int TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public int Numero_Credito { get; set; }
        public int Plazo { get; set; }
        public decimal TasaEA { get; set; }
        public decimal Tasa_Seguro_Hogar { get; set; }
        public decimal Tasa_Seguro_Vida { get; set; }
    }
}
