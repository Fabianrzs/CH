using System;

namespace GA2.Domain.Entities
{
    public class ReporteMensualSeguros
    {
        public long NUMERO_RADICACION { get; set; }
        public string TIPO_CREDITO { get; set; }
        public decimal VALOR_DESEMBOLSO { get; set; }
        public decimal VALOR_INMUEBLE { get; set; }
        public DateTime FECHA_APERTURA { get; set; }
        public DateTime FECHA_TERMINACION { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public string DIRECCION_INMUEBLE { get; set; }
        public string MATRICULA_INMUEBLE { get; set; }
        public string TIPO_VIVIENDA { get; set; }
        public decimal? VALOR_AVALUO_COMERCIAL { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public DateTime? FECHA_EXPEDICION_DOCUMENTO { get; set; }
        public DateTime? FECHA_NACIMIENTO { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string CORREO { get; set; }
        public string GRADO { get; set; }
        public string CATEGORIA { get; set; }
        public decimal SEGURO_HOGAR { get; set; }
        public decimal SEGURO_VIDA { get; set; } 
    }

}
