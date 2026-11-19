using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class PostulanteResponse
    {
        public Guid IdSolicitud { get; set; }
        public string NombrePostulante { get; set; }
        public string ApellidoPostulante { get; set; }
        public string CodigoIdentificacion { get; set; }
        public string nombreIdentificacion { get; set; }
        public long NumeroDocumentoPostulante { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public int parentesco { get; set; }
        public string Sexo { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string EstadoCivil { get; set; }
        public int Ocupacion { get; set; }
        public string NivelEducativo { get; set; }
        public string AfectacionFisica { get; set; }
        public string AfectacionSensorial { get; set; }
        public string EnfermedadesEspecificas { get; set; }
        public string TratamientoMedico { get; set; }
        public string DificultaEscaleras { get; set; }
        public string MedicamentoPermanente { get; set; }
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
