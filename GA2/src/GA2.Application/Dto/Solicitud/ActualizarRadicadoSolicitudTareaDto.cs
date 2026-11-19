using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class ActualizarRadicadoSolicitudTareaDto
    {
        public int tipoIdentificacion { get; set; }
        public string identificacionCliente { get; set; }
        public string CodProcesoGD { get; set; }
        public string CodSubModGD { get; set; }
        public int NumeroSolicitud { get; set; }
        public string Radicado { get; set; }
        public string TipoSolicitud { get; set; }
        public string TipoTurno { get; set; }
        public string NumeroTurno { get; set; }
        public string CanalRecepcion { get; set; }
        public string Sucursal { get; set; }
        public string TipoVivienda { get; set; }
        public Guid CreadoPor { get; set; }
        public int Numero_Folio { get; set; }
        public DateTime? Fecha_Firma_Escritura { get; set; }


    }
}
