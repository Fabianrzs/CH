using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class CuentaClienteDto
    {
        public int CuentaId { get; set; }
        public int CuentaIdIntegracion{ get; set; }
        public int NumeroCuenta { get; set; }
        public int ClinteId { get; set; }
        public int TipoCuentaId { get; set; }
        public int EstadoCuentaId { get; set; }
        public string EstadoCuenta { get; set; }
        public int CausalEstadoId { get; set; }
        public int DocumentoId { get; set; }
        public DateTime FechaCreacion{ get; set; }
        public DateTime FechaCierre { get; set; }
        public DateTime FechaPrimerAporte { get; set; }
        public decimal Saldo{ get; set; }
        public double SaldoCanje { get; set; }
        public decimal SaldoDisponible { get; set; }
        public int Cueotas { get; set; }
        public double Embargos { get; set; }
        public bool CuentaBloqueo { get; set; }
        public List<MovimientosCuentaDto> Movimientos { get; set; }
        public List<ConceptoCuentaDto> Conceptos { get; set; }
        public List<EmbargosDto> Concepto { get; set; }
        public List<BloqueoCuentaConceptoDto> Bloqueos { get; set; }
        public List<InclusionSaldoRequestDto> InclusionSaldos { get; set; }
        public string NombreCuenta { get; set; }
        public List<EmbargoCuentaConceptoDto> ListaEmbargos { get; set; }
        public decimal AsignacionSalarial { get; set; }
        public decimal Porcentaje { get; set; }

    }

    public class informaClienteDto {

        public int ClienteId { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Identificacion { get; set; }
        public int TipoIdentificacionId { get; set; }
        public int CategoriaId { get; set; }
        public int FuerzaId { get; set; }
        public int GradoId { get; set; }
        public int UnidadEjecutoraId { get; set; }
        public string Categoria { get; set; }
        public string Fuerza { get; set; }
        public string Grado { get; set; }
        public string UnidadEjecutora { get; set; }
        public string TipoIdentificacion { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public int TipoAfiliacionId { get; set; }
        public string TipoAfiliacion { get; set; }
        public CuentaClienteDto cuentas { get; set; }

    }


}
