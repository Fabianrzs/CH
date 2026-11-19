using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class EstadoReintegros
    {
        public int RT_ESTADO_ID { get; set; }
        public string DESCRIPCION { get; set; }
        public bool ESTADO { get; set; }
        public Guid CREADO_POR { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public Guid MODIFICADO_POR { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
