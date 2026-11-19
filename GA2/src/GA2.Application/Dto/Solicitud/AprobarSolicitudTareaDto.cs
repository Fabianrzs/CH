using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class AprobarSolicitudTareaDto
    {
        public Guid solicitudTareaId { get; set; }
        public int clienteId { get; set; }
        public int? SLT_ESTADO_CLIENTE { get; set; }
        public int? SLT_ESTADO_CUENTA { get; set; }
        public string sOL_RECHAZADA { get; set; }
        public int NivelRiesgo { get; set; } = 0;
    }
}
