using System;
using System.Collections.Generic;

namespace GA2.Domain.Core
{
    public class CierreCausacionResponseDto
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaProceso { get; set; }
        public int TotalCreditosProcesados { get; set; }
        public decimal TotalCausacionGenerada { get; set; }
        public int DuracionSegundos { get; set; }
        public List<string> Errores { get; set; } = new();
    }





    
}

