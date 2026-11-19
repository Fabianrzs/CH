using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class PeticionConsultarSolicitudCesantiasDto
    {
        // public DateTime? FechaSolicitud { get; set; }
        public string? Identificacion { get; set; }
        public int? EstadoTarea { get; set; } //Pendiente, En tramite, Subsanacion, Aprobada, Rechazada
        public Guid? TipoSolicitud { get; set; } 
        public string? NumRadicado { get; set; }
        public DateTime? FechaIni { get; set; }
        public DateTime? FechaFin { get; set; }
        public Guid? Modalidad { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } =10000;
        public string NumeroSolicitud { get; set; }
        public string UsuarioIngreso { get; set; }
        public string solicitante { get; set; }
        public Guid? submodalidad { get; set; }
       // public Guid? idUsuario { get; set; }



    }
}
/*	
 *	parametros del SP
 *	@PCS_ID 				VARCHAR(max),
	@SLT_ESTADO_SOLICITUD 	INT,
	@NumRadicado varchar(30),
	@FechaIni datetime,
	@FechaFin datetime,
	@Tipo uniqueidentifier,
	@Modalidad uniqueidentifier
*/