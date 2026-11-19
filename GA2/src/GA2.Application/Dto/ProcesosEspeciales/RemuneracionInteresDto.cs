using System;


namespace GA2.Application.Dto
{    
    /// <summary>
     /// Dto de procesos espaciales  liquidacion de intereses
     /// <author>Cesar Hernandez</author>
     /// <date>14/03/2023</date>
     /// </summary>
    public class RemuneracionInteresDto
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public float ValorIPC { get; set; }
        public float PorcentajeIPC { get; set; }

    }
}
