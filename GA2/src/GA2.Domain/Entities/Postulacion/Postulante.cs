using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class Postulante
    {
        public Guid? POS_ID { get; set; }
        public Guid POS_ID_SOLICIUD { get; set; }
        public string POS_NOMBRE { get; set; }
        public string POS_APELLIDO { get; set; }
        public int POS_ID_TIPO_DOCUMENTO { get; set; }
        public long POS_NUMERO_DOCUMENTO { get; set; }
        public int POS_DEPARMENTO_EXPEDICION { get; set; }
        public int POS_CIUDAD_EXPEDICION { get; set; }
        public int POS_PARENTESCO { get; set; }
        public string POS_SEXO { get; set; }
        public DateTime? POS_FECHA_NACIMIENTO { get; set; }
        public string POS_ESTADO_CIVIL { get; set; }
        public int POS_OCUPACION { get; set; }
        public int POS_NIVEL_EDUCATIVO { get; set; }
        public string POS_AFECTACION_FISICA { get; set; }
        public string POS_AFECTACIONSENSORIAL { get; set; }
        public string POS_ENFERMEDADESPECIFICA { get; set; }
        public string POS_TRATAMIENTOMEDICO { get; set; }
        public string POS_DIFICULTADESCALERAS { get; set; }
        public string POS_MEDICAMENTOPERMANENTE { get; set; }
        public string POS_AYUDATECNICASENSORIAL { get; set; }
        public string POS_CONDICIONSALUD { get; set; }
        public string TipoIntegranteId { get; set; }
        public string TipoParentescoId { get; set; }
        public string TipAyudaTecnicaId { get; set; }
        public string TipoDiscapacidadId { get; set; }
        public int? PorcentajePostulacion { get; set; }
        public int? PorcentajeCesantias { get; set; }
        public int? PorcentajeAportes { get; set; }
    }
}
