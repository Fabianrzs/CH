using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class ReporteAfiliadosCesantiasRequestDto
    {
        public string TipoCuenta { get; set; }
        public DateTime? FechaRadicacionInicial { get; set; }
        public DateTime? FechaRadicacionFinal { get; set; }
    }

    public class ReporteAfiliadosCesantiasItem
    {
        public string IdFuerza { get; set; }
        public string IdUnidadEjecutora { get; set; }
        public string IdCategoria { get; set; }
        public string IdGrado { get; set; }
        public string TipoActor { get; set; }
        public string TipoModalidad { get; set; }
        public string TipoSolicitud { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string CTC_DESCRIPCION { get; set; }
        public decimal CAC_VALOR_DESCONTADO { get; set; }
        public string NUMERO_CUENTA { get; set; }
        public string DESCRIPCION_CUENTA { get; set; }

    }

    public class ReporteAfiliadosCesantiasResponse
    {
        public List<ReporteAfiliadosCesantiasItem> Afiliados { get; set; } = new();
    }

}
