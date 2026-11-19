using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class BeneficiariosAfiliacionesDto
    {
        public int idDependiente { get; set; }
        public int idBeneficiarioAfiliacion { get; set; }
        public int idCliente { get; set; }
        public int idTipoIdentificacion { get; set; }
        public string tipoIdentificacion { get; set; }
        public string identificacion { get; set; }
        public string nombre { get; set; }
        public int participacion { get; set; }
        public string direccion { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public bool conservaAntiguedad { get; set; }
        public int numeroActoAdministrativoResolucionesReconocimientoPensional { get; set; }
        public DateTime fechaNotificacionResolucionesReconocimientoPensional { get; set; }
        public int porcentajeReconocimientoPensionalDistribucionAportes { get; set; }
        public int numeroActoAdministrativoResolucionesReconocimientoPrestacionesSociales { get; set; }
        public DateTime fechaNotificacionResolucionesReconocimientoPrestacionesSociales { get; set; }
        public int porcentajeReconocimientoPrestacionesSocialesDistribucionCesantias { get; set; }
        public bool derechoLiquidacionCuotasBeneficiarios { get; set; }
        public int porcentajeReconocimientoPensionalDistribucionSubsidio { get; set; }
    }
}
