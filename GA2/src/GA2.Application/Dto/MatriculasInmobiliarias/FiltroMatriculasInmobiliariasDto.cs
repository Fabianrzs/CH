using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class FiltroMatriculasInmobiliariasDto
    {
        public string NumeroMatricula { get; set; }
        public string NumeroIdentificacion { get; set; }
        public int TipoIdentificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaFin { get; set; }

        public int ofc_id { get; set; }
        public string nomOfc { get; set; }
        public int bloqueo { get; set; }    
    }
}
