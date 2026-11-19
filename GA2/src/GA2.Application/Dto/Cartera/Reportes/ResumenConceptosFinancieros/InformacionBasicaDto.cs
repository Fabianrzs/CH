using System;

namespace GA2.Application.Dto.Cartera.Reportes.ResumenConceptosFinancieros
{
    public class InformacionBasicaDto
    {
        public string NOMBRES_COMPLETOS { get; set; }
        public int EDAD { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public string SEXO { get; set; }
        public DateTime FECHA_NACIMIENTO { get; set; }
        public string LUGAR_NACIMIENTO { get; set; }
        public string DIRECCION_RESIDENCIA { get; set; }
        public string TELEFONO_CELULAR { get; set; }
        public string TELEFONO_FIJO { get; set; }
        public string EMAIL { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public string CATEGORIA { get; set; }
        public string GRADO { get; set; }
        public string CUOTAS_APORTADAS { get; set; }
        public string FECHA_REPORTE { get; set; }
    }
}
