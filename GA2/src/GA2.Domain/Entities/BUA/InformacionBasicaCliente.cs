using System;

namespace GA2.Domain.Entities.BUA
{
    public class InformacionBasicaCliente
    {
        public int ID_CLIENTE { get; set; }
        public int TIPO_IDENTIFICACION { get; set; }
        public string IDENTIFICACION { get; set; }
        public DateTime? FECHA_EXPEDICION_DOCUMENTO { get; set; }
        public DateTime? FECHA_NACIMIENTO { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string NOMBRE_COMPLETO_INVERTIDO { get; set; }
        public string PRIMER_NOMBRE { get; set; }
        public string SEGUNDO_NOMBRE { get; set; }
        public string PRIMER_APELLIDO { get; set; }
        public string SEGUNDO_APELLIDO { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string CORREO { get; set; }
        public string ID_FUERZA { get; set; }
        public string ID_GRADO { get; set; }
        public string ID_CATEGORIA { get; set; }
        public string SEXO { get; set; }
        public string ID_UNIDADEJECITORA { get; set; }
        public string TipoVinculacion { get; set; }
        public int ID_DEPARTAMENTO { get; set; }
        public int ID_CIUDAD { get; set; }
    }
}
