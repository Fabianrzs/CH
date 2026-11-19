using System;


namespace GA2.Application.Dto
{
    public class AfiliadoReintegroDto
    {
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public int IdTipoReintegro { get; set; }
        public string TipoReintegro { get; set; }
        
    }

    public class AfiliadoReintegroParamDto
    {
        public string Identificacion { get; set; }
        public int IdTipoReintegro { get; set; }
    }
}
