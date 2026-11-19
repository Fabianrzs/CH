using System;

namespace GA2.Domain.Entities
{
    public class ParamCierreMensualCartera
    {
        public int AnioCierre { get; set; }
        public int MesCierre { get; set; }
        public bool Actualiza { get; set; }
        public Guid CreadoPor { get; set; }
    }

    public class CierreMensualCartera
    {
        public string MesCierre { get; set; }
        public bool Actualiza { get; set; }
    }
}
