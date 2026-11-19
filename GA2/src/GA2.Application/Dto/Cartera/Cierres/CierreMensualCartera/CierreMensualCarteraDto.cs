using System;

namespace GA2.Application.Dto
{

    public class ParamCierreMensualCarteraDto
    {
        public int AnioCierre { get; set; }
        public int MesCierre { get; set; }
        public bool Actualiza { get; set; }
        public Guid CreadoPor { get; set; }
    }
    public class ParamCierreMensualCarteraSevenDto
    {
        public int ID { get; set; }
        public int AnioCierre { get; set; }
        public int MesCierre { get; set; }
    }
    public class CierreMensualCarteraDto
    {
        public string MesCierre { get; set; }
        public bool Actualiza { get; set; }
    }

    public class ExtractoMensualCarteraDto
    {
        public string Identificacion { get; set; }
        public string Periodo { get; set; }
    }

    public class ExtractoAnualCarteraDto
    {
        public string Identificacion { get; set; }
        public string Periodo { get; set; }
    }

    public class ParamReporteCierreMensualCarteraDto
    {
        public int AnioCierre { get; set; }
        public int MesCierre { get; set; }
    }
}