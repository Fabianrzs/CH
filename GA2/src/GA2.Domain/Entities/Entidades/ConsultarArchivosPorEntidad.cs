using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ConsultarArchivosPorEntidad
    {
        public Guid ENE_PAM_ID { get; set; }
        public string ENE_PAM_NOMBRE_ARCHIVO { get; set; }
        public string ENE_PAM_DESCRIPCION_ARCHIVO { get; set; }
        public bool ENE_PAM_REQUERIDO { get; set; }
        public int ENE_PAM_ORDEN { get; set; }
        public bool ENE_PAM_MULTIPLE_ARCHIVO { get; set; }
        public string ENE_PAM_FORMATO { get; set; }
        public bool ENE_PAM_ESTADO { get; set; }
        public int CVL_ID { get; set; }
        public string CVL_DESCRIPCION { get; set; }
        public Guid ENE_ARCHIVOS_ID { get; set; }
        public string ENE_NOMBRE_ARCHIVO { get; set; }
        public string ENE_RUTA_STORAGE { get; set; }
        public string ENE_EXTENSION { get; set; }
        public string ENE_PAM_NUMERO_FOLIO { get; set; }
        public DateTime ENE_FECHA_CARGUE { get; set; }
    }
}
