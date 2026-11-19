using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Transversals.Commons.Configuraciones
{
    /// <summary>
    /// IOptions para la configuracion url svc servicios CajaHonor
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>21/06/2021</date>
    public class ConfiguracionesSvc
    {
        public string BuaUrlSvc { get; set; }
        public string WorkManagerUrlSvc { get; set; }
        public string DodoUrlSvc { get; set; }
        public string HaberesUrlSvc { get; set; }
        public string VigiaCargaTerceroUrlSvc { get; set; }
        public string VigiaCargaProductosTerceroUrlSvc { get; set; }
        public string VigiaCargaTransaccionUrlSvc { get; set; }
        public string VigiaVerificarTerceroUrlSvc { get; set; }

        public string SevenWSFactura { get; set; }
        public string SevenWSCdp { get; set; }
        public string SevenWintipro { get; set; }
        public string SevenWSMovContable { get; set; }
        public string SevenWSProveedor { get; set; }
        public string SevenWSTercero { get; set; }
        public string SevenWSAportes { get; set; }

        public string FenixServiceSvc { get; set; }
        public string ServiceOtpSvc { get; set; }
        public string LDAP { get; set; }
        public string TransUnion { get; set; }

        public string BuaUrlServiceApi { get; set; }
    }
}
