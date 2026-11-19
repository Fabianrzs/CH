using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.VerificacionDerechos
{

    public class InteresesCausadosNoCausadosInfoDto
    {
        public long NumeroSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public string TipoConcepto { get; set; }
        public decimal Valor { get; set; }

        public string Periodo { get; set; }
        public string SolicitudModo { get; set; }
        public string TipoInteres { get; set; }
        public DateTime FechaSolicitud { get; set; }

        


    }





}
