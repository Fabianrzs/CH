using System;

namespace GA2.Domain.Entities
{
    public class ErrorCargueArchivo
    {
        public Guid NMN_ID { get; set;}
        public int DCT_ID { get; set; }
        public int NUMERO_LINEA { get; set; }
        public int INCONSISTENCIAS_NOMBRE { get; set; }
        public int INCONSISTENCIAS_ENCABEZADO { get; set; }
        public int INCONSISTENCIAS_CUERPO { get; set; }
        public int INCONSISTENCIAS_FIN { get; set; }
        public int CODIGO_INCIDENCIA { get; set; }
        public string DESCRIPCION { get; set; }
        public string TIPO_ERROR { get; set; } 
        public DateTime FECHA_CREACION { get; set; }
        public int ESTADO_ERROR { get; set; }
        public ErrorCargueArchivo() { }
        public ErrorCargueArchivo(int dct_Id,int numero_Linea, int inconsistencias_nombre, int inconsistencias_encabezado, int inconsistencias_cuerpo, int inconsistencias_fin, int codigo_Incidencia, string descripcion, string tipo_Error, int estado_error)
        {
            DCT_ID = dct_Id;
            NUMERO_LINEA = numero_Linea;
            INCONSISTENCIAS_NOMBRE = inconsistencias_nombre;
            INCONSISTENCIAS_ENCABEZADO = inconsistencias_encabezado;
            INCONSISTENCIAS_CUERPO = inconsistencias_cuerpo;
            INCONSISTENCIAS_FIN = inconsistencias_fin;
            CODIGO_INCIDENCIA = codigo_Incidencia;
            DESCRIPCION = descripcion;
            TIPO_ERROR = tipo_Error;
            ESTADO_ERROR = estado_error;
        }

    }
}
