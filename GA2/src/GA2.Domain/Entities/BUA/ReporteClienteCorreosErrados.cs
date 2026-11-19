using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.BUA
{
    public class ReporteClienteCorreosErrados
    {
        public int CLI_ID {get; set;}
        public int TID_ID  {get; set;}
        public String CLI_IDENTIFICACION  { get; set; }
        public String CLI_SEGUNDO_APELLIDO { get; set; }
        public String CLI_PRIMER_APELLIDO  { get; set; }
        public String CLI_PRIMER_NOMBRE  { get; set; }
        public String CLI_SEGUNDO_NOMBRE  { get; set; }
        public DateTime CLI_FECHA_INSCRIPCION  { get; set; }
        public DateTime CLI_FECHA_ALTA  { get; set; }
        public String TIPO_VINCULACION { get; set; }
        public String ESTADO_CLIENTE { get; set; }
        public String CLI_REGIMEN { get; set; }
        public String FUERZA { get; set; }
        public String CATEGORIA { get; set; }
        public String UNIDAD_EJECUTORA { get; set; }
        public int DCE_ID  { get; set; }
        public int DRC_ID   { get; set; }
        public String DCE_CORREO { get; set; }
        public int UEJ_ID { get; set; }
    }
}
