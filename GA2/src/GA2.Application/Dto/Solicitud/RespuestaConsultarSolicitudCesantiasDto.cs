using System;

namespace GA2.Application.Dto
{
    public class RespuestaConsultarSolicitudCesantiasDto
    {
        public int NumeroSolicitud { get; set; }
        public string ProcesoDescripcion { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string CedulaCliente { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string? FechaSolicitudFormateada { get; set; }
        public decimal ValorRetirar { get; set; } //SOL_VALOR_A_RETIRAR
        public string TareaNombre { get; set; }
        public string EstadoSolicitud { get; set; }
        public int EstadoSolicitudId { get; set; }
        public Guid SolicitudTareaId { get; set; }
        public Guid? IdUsuarioAsignado { get; set; }
        public string? UsuarioAsignado { get; set; }

        public int IdCliente { get; set; }
        public int? NuNUMFOLIOS { get; set; }
        public string TimCodigo { get; set; }
        public string TpsSubId { get; set; }
        public string TpsSubDescripcion { get; set; }
        public string RadicadoGestionDocumental { get; set; }
        public string NumeroRadicadoSG { get; set; }
        public int? NUMFOLIOS { get; set; }

        public string SOL_RECHAZADA { get; set; }

        public string ModalidadDescripcion {get; set;}
    }
}