using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Proyecto
{
    public class ProyectoConstructoraResponseDto
    {
        public Guid Codigo { get; set; }
        public string NombreProyecto { get; set; }
        public int DepartamentoId { get; set; }
        public string Direccion { get; set; }
        public string Deparmento { get; set; }
        public int MunicipioId { get; set; }
        public string Ciudad { get; set; }
        public long Telefono { get; set; }
        public double Valor { get; set; }
        public int Cupos { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int EstadoId { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string Objeto { get; set; }
        public int TipoontratoId { get; set; }
        public string TipoContrato { get; set; }
        public string NumeroRegistro { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public Guid ConstructoraId { get; set; }
        public string Constructora { get; set; }
        public int TipoProyecto { get; set; }
        public LicenciaUrbanismoResponseDto LicenciaUrbanismo { get; set; }
        public LicenciaUrbanismoResponseDto LicenciaConstruccion{ get; set; }
        public List<SupervisorResponseDto> supervisors { get; set; }  
        public List<BeneficiarioResponseDto> beneficiario { get; set; }
        public List<PolizaProyectoResponseDto> polizas { get; set; }
        public List<ClaseInmuebleResponseDto> ClasesInmuebles { get; set; }
        public List<ListaAvanceResponseDto> ListaAvance { get; set; }
        public List<InmublesProyectoResponseDto> InmublesProyectos { get; set; }
    }
}
