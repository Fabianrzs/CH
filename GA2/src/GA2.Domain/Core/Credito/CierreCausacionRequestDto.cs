using System;

namespace GA2.Domain.Core
{
    // =====================================================
    // DTOs PARA CAUSACIÓN CON VALIDACIÓN
    // =====================================================

    public class CierreCausacionRequestDto
    {
        public DateTime FechaCierre { get; set; }
        public bool ProcesarHipotecarios { get; set; } = true;
        public bool ProcesarLeasing { get; set; } = true;
    }





    
}

