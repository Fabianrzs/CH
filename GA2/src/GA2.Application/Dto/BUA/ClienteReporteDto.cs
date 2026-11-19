using GA2.Application.Dto.Catalogs;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class ClienteReporteDto
    {
        public DateTime FechaInscripcion { get; set; }
        public DateTime FechaAlta { get; set; }
    }

    public class ClienteNominaDto
    {
        public int IdCliente { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string TipoVinculacion { get; set; }
        public int IdFuerza { get; set; }
        public int IdGrado { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public string? EstadoCliente { get; set; }
        public int? IdEstadoCliente { get; set; }
        public int CuentasHabilitadas { get; set; }
        public int NumeroCuotas { get; set; }
        public List<CuentaClienteDto> Cuentas { get; set; }
        public List<AportesClienteDto> AportesCategoria { get; set; }

        public bool isSuccess { get; set; }
        public string returnMessage { get; set; }
    }
}
