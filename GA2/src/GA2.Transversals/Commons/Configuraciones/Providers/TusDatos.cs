using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Commons.Configuraciones.Providers
{
    public class TusDatos
    {
        public class TusDatosSettings
        {
            public string BaseUrl { get; set; } = default!;
            public string ApiKey { get; set; } = default!;
            public string Version { get; set; } = default!;
            public string Controller { get; set; } = default!;
            public int TimeoutSeconds { get; set; } = 15;

            public TusDatosEndpoints Endpoints { get; set; } = new();
        }

        public class TusDatosEndpoints
        {
            public string ConsultarCliente { get; set; } = default!;
            public string ConsultarJob { get; set; } = default!;
            public string ConsultarVigia { get; set; } = default!;
        }
    }
}
