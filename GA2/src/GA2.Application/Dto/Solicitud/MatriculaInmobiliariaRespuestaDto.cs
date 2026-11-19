using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class MatriculaInmobiliariaRespuestaDto
    {
        public Guid MaiId { get; set; }
        public string MaiNumeroMatricula { get; set; }
        public string MaiDireccion { get; set; }
        public DateTime MaiFechaRegistro { get; set; }
        public int DpdId { get; set; }
        public int DpcId { get; set; }
        public bool MaiPrincipal { get; set; }
        public bool MaiEstado { get; set; }
        public bool MaiBloqueo { get; set; }
        public int OfcId { get; set; }
        public string OfcDescripcion { get; set; }
        public string DpdOficinaRegistro { get; set; }
        public string DpdNombre { get; set; }
        public string DpcNombre { get; set; }
        public int DpdOfcId { get; set; }
        public string OfcCodigo { get; set; }
        public int Tramitada { get; set; }
    }
}