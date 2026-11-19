using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.MatriculasInmobiliarias
{
    public class HistorialCambiosMatriculasDto
    {
       public Guid matcId { get; set; }
       public Guid maiId {get ;set;}
       public string matcPestaña {get ;set;}
       public string matcCampo {get ;set;}
       public string matcVlrAnterior { get; set; }

       public string matcVlrNuevo { get ;set;} 
       public string match_user {get ;set;}
       public DateTime matc_fecmood {get ;set;}
       public string usuarioBloqueado { get; set; }

    }
}
