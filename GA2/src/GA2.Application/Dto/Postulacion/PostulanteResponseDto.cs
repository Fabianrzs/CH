using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Postulacion
{
    public class PostulanteResponseDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int TipoDocumentoId { get; set; }
        public long NumeroDocumento { get; set; }
        public int DepartamentoExpedicion { get; set; }
        public int CiudadExpedicion { get; set; }
        public int Parentesco { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string EstadoCivil { get; set; }
        public int Ocupacion { get; set; }
        public int NivelEducativo { get; set; }
        public string AfectionFisica { get; set; }
        public string AfectacionSensorial { get; set; }
        public string EnfermedadEspecifica { get; set; }
        public string TratamientoMedico { get; set; }
        public string DificultadEscaleras { get; set; }
        public string MedicoPermanente { get; set; }
        public string AyudaTecnica { get; set; }
        public string CondicionSalud { get; set; }
        public string TipoIntegranteId { get; set; }
        public string TipoParentescoId { get; set; }
        public string TipAyudaTecnicaId { get; set; }
        public string TipoDiscapacidadId { get; set; }
        public int? PorcentajePostulacion { get; set; }
        public int? PorcentajeCesantias { get; set; }
        public int? PorcentajeAportes { get; set; }

    }
}
