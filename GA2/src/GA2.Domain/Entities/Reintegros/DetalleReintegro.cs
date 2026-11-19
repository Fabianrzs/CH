using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class DetalleReintegro
    {
        public int RT_REI_DET_ID { get; set; }
        public int RT_REI_ID { get; set; }
        public string IDENTIFICACION { get; set; }
        public Guid CREADO_POR { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public Guid MODIFICADO_POR { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
