using GA2.Application.Dto.Credito.Simulador.Comite;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class SolicitudActaComiteDto
    {
        public List<DatosComiteCreditoDto> DatosComite { get; set; }
        public IEnumerable<IntegrantePorComiteDto> IntegrantesComite { get; set; }
        public IEnumerable<TemaComiteCreditoDto> TemasComite { get; set; }
        public IEnumerable<SolicitudComiteDto> Solicitudes { get; set; }
        public IEnumerable<DatosSolicitudComiteDto> DatosSolicitud { get; set; }
        public List<SolicitudComiteInfInmuebleDto> DatosInmueble { get; set; }
        public List<SolicitudComiteInfFinancieraDto> DatosFinanciera { get; set; }

        public UsuarioDto Usuario { get; set; }

    }
}
