using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class InformacionAdjudicacion
    {
        public string? TipoVivienda { get; set; }
        public string? Direccion { get; set; }
        public string? MatriculaInmobiliaria { get; set; }
        public string? ValorSubsidio { get; set; }
        public string? SIA_CIUDAD { get; set; }
        public Guid? SIA_CREATEDOPOR { get; set; }
        public Guid? SOL_ID_FK { get; set; }

    }
}
