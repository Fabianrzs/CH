using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class AprobarSolicitudTarea
    {
        public Guid SLT_ID { get; set; }
        public int SLT_ESTADO_SOLICITUD { get; set; }
        public int SLT_ESTADO_SOLICITUD_NUEVO { get; set; }
        public int SLT_ESTADO_CLIENTE { get; set; } = 0;
        public int SLT_ESTADO_CUENTA { get; set; } = 0;
        public int FRZ_CODIGO { get; set; } = 0;
        public string? NOMBRE_COMPLETO { get; set; } = null;
        public string? NUM_CEDULA { get; set; } = null;
        public int? IdGrado { get; set; }
        public int Prorrateo { get; set; }
        public int ReglaRegimenCuota {get; set; }
        public string SOL_RECHAZADA { get; set; }
        public int EmbargoBloqueo { get; set; }
        public int NivelRiesgo { get; set; } = 0;
    }
}
