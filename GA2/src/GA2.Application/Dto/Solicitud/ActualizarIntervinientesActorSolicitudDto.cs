using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class ActualizarIntervinientesActorSolicitudDto
    {
        public Guid IntervinientesId { get; set; }
        public Guid ModificadoPor { get; set; }
        public string? Nombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? primerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Modulo { get; set; }
        public string Celular { get; set; }
        public string CorreoElectronico { get; set; }
        public int Departamento { get; set; }
        public int Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Barrio { get; set; }
        public string Telefono { get; set; }
        public string NumeroDocumentoAfiliado { get; set; }
        public int TipoDocumentoAfiliado { get; set; }
    }
}
