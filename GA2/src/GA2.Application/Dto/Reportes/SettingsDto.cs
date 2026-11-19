using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Reportes
{
    public  class SettingsDto
    {
        public Guid Id { get; set; }
        public String IdReporte { get; set; }

        public String NombreReporte { get; set; }

        public String NombreCortoReporte { get; set; }
    }
}
