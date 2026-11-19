using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{

public class Recepcion
    {
        public Guid RIT_ID { get; set; }
        public string RIT_DESCRIPCION { get; set; }
        public string RIT_CODIGO { get; set; }
    }

    public class Sucursal
    {
        public Guid SIT_ID { get; set; }
        public string SIT_DESCRICION { get; set; }
        public string SIT_CODIGO { get; set; }
        public string SIT_DESCRIPCION_DODO { get; set; }
    }

    public class Turno
    {
        public Guid TIT_ID { get; set; }
        public string TIT_IDITEM { get; set; }
        public string TIT_IDLISTA { get; set; }
        public string TIT_CODIGO { get; set; }
        public string TIT_NOMBRE { get; set; }
        public string TIT_ID_ITEM_PADRE { get; set; }
        public string TIT_ACTIVO { get; set; }
        public string TIT_USUARIO { get; set; }
        public DateTime TIT_FECHA { get; set; }
        public string TIT_HOST { get; set; }
        public string TIT_TOOL_TIP_TEXT { get; set; }
    }

    public class InformacionTurno 
    {
        public Guid SIT_ID { get; set; }
        public Guid RIT_ID_FK { get; set; }
        public Guid SIT_ID_FK { get; set; }
        public Guid TIT_ID_FK { get; set; }
        public string SIT_PRE_TRAMITE { get; set; }
        public string SIT_TURNO_NUMERO { get; set; }
        public string SIT_NUMERO_COMISION { get; set; }
        public DateTime SIT_FECHA_COMISION { get; set; }
        public Guid SOL_ID_FK { get; set; }
    }


}
