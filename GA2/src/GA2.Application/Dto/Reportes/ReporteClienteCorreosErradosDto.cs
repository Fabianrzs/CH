using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Reportes
{
    public  class ReporteClienteCorreosErradosDto
    {
        public int ID_CLIENTE {  get; set; }
        public int TIPO_IDENTIFICACION  {  get; set; }
        public String IDENTIFICACION   {  get; set; }
        public String SEGUNDO_APELLIDO { get; set; }
        public String PRIMER_APELLIDO  { get; set; }
        public String PRIMER_NOMBRE  { get; set; }
        public String SEGUNDO_NOMBRE  { get; set; }
        public DateTime FECHA_INSCRIPCION  { get; set; }
        public DateTime FECHA_ALTA  { get; set; }
        public String TIPO_VINCULACION { get; set; }
        public String ESTADO_CLIENTE { get; set; }
        public String REGIMEN { get; set; }
        public String FUERZA { get; set; }
        public String CATEGORIA { get; set; }
        public String UNIDAD_EJECUTORA { get; set; }
        public int DCE_ID   { get; set; }
        public int DRC_ID  { get; set; }
        public String CORREO { get; set; }                
        public int UEJ_ID { get; set; }

    }
}
