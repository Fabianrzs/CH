using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class DatosPersonalesCreditoEducativoDto
    {
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string PrimerNombreEstudiante { get; set; }
        public string SegundoNombreEstudiante { get; set; }
        public string PrimerApellidoEstudiante { get; set; }
        public string SegundoApellidoEstudiante { get; set; }
        public string DireccionEstudiante { get; set; }
        public decimal ValorDeCredito { get; set; }
        public string PeriodoLectivo { get; set; }
        public Guid EntidadEducativaID { get; set; }
        public int DepartamentoID { get; set; }
        public int CiudadID { get; set; }
        public Guid SLS_ID { get; set; }
        public string NumeroIdentificacionTitular { get; set; }
    }
}
