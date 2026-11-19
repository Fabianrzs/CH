using GA2.Domain.Entities.Solicitud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class InformacionTurnoDto
    {
        public Guid? TipoRecepcion { get; set; }
        public string? RadPreTramite { get; set; }
        public string? NumeroComision { get; set; }
        public DateTime? FechaComision { get; set; }
        public Guid? TipoTurno { get; set; }
        public string? Turno { get; set; }
        public Guid? TipoSucursal { get; set; }
    }

    public class InformacionTurnoResponse
    {
        public List<Turno> Turnos { get; set; }
        public List<Sucursal> Sucursales { get; set; }
        public List<Recepcion> Recepciones { get; set; }
    }
}
