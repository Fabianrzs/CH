using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Proyecto
{
    public class ProyectoConstructoraModel
    {
        public Guid? PC_PROYECTO_ID { get; set; } = Guid.Empty;
        public Guid PC_CONTRUCTORA_ID { get; set; }
        public string PC_NOMBRE { get; set; }
        public string PC_DIRECCION { get; set; }
        public int PC_DEPARTAMENTO_ID { get; set; }
        public int PC_MUNICIPIO_ID { get; set; }
        public long PC_TELEFONO { get; set; }
        public double PC_VALOR { get; set; }
        public int PC_CUPOS { get; set; }
        public double PC_LATITUD { get; set; }
        public double PC_LONGITUD { get; set; }
        public int PC_ESTADO_ID { get; set; }
        public DateTime PC_FECHA_CREACION { get; set; }
        public DateTime PC_FECHA_VIGENCIA { get; set; }
        public string PC_OBJETO { get; set; }
        public int PC_TIPO_CONTRATO_ID { get; set; }
        public string PC_NUMERO_REGISTRO { get; set; }
        public DateTime PC_FECHA_ACTA_INICIO { get; set; }
        public DateTime PC_FECHA_ENTREGA { get; set; }
        public int PC_TIPO_PROYECTO { get; set; }
    }
}
