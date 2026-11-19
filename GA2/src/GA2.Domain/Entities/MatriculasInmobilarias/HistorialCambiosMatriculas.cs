using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.MatriculasInmobilarias
{
    public class HistorialCambiosMatriculas
    {
        public Guid MATC_ID { get; set; }
        public Guid MAI_ID {get; set ;}
        public string MATC_PESTAÑA {get; set ;}
        public string MATC_CAMPO {get; set ;}
        public string MATC_VLR_ANTERIOR {get; set ;}
        public string MATC_VLR_NUEVO {get; set ;}
        public string MATC_USER {get; set ;}
        public DateTime MATC_FECMOD {get; set ;}
        public string USUARIOBLOQUEO { get; set ; }

    }
}
