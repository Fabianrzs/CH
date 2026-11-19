using System;

namespace GA2.Application.Dto.Cartera.Novedades
{
    // ================================================================================
    // DTO PARA RESPUESTA DE SOLICITUD DE REESTRUCTURACIÓN
    // ================================================================================
    public class SolicitudReestructuracionDto
    {
        public long NumeroCredito { get; set; }
        public decimal MontoMinimo { get; set; }
        public decimal InteresesCorrientes { get; set; }
        public decimal InteresesMora { get; set; }
        public DateTime FechaLimitePago { get; set; }
    }
}
