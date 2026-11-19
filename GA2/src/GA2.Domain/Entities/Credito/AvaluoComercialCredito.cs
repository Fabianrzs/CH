using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class AvaluoComercialCredito
    {
       
            public Guid AVC_ID { get; set; }

            public Guid SOC_ID { get; set; }

            public DateTime AVC_FECHA { get; set; }

            public bool AVC_ESTADO { get; set; }

            public string AVC_CLIENTE { get; set; }

            public string AVC_NUMERO_DOCUMENTO { get; set; }

            public string AVC_DIRECCION { get; set; }

            public string AVC_SECTOR { get; set; }

            public string AVC_BARRIO { get; set; }

            public int? AVC_ID_DPD { get; set; }

            public int? AVC_ID_DPC { get; set; }

            public DateTime? AVC_FECHA_INFORME { get; set; }

            public string AVC_VETUSTEZ { get; set; }

            public DateTime? AVC_FECHA_PAGO_AVALUO { get; set; }

            public DateTime? AVC_FECHA_VISITA { get; set; }

            public string AVC_PROPIETARIO { get; set; }

            public string AVC_ESCRITURA { get; set; }

            public string AVC_NOTARIA { get; set; }

            public DateTime? AVC_FECHA_ESCRITURA { get; set; }

            public int? AVC_DPD_ID_ESCRITURA { get; set; }

            public int? AVC_DPC_ID_ESCRITURA { get; set; }

            public string AVC_CEDULA_CATASTRAL { get; set; }

            public string AVC_LICENCIA_CONSTRUCCION { get; set; }

            public string AVC_NOMBRE_CONJUNTO { get; set; }

            public string AVC_COEFICIENTE_COPROPIEDAD { get; set; }

            public string AVC_DISTRIBUCION_INMUEBLE { get; set; }

            public string AVC_CARACTERISTICAS_ADICIONALES { get; set; }

            public string AVC_DESCRIPCION_DEPENDENCIA { get; set; }

            public string AVC_GARAJE { get; set; }

            public string AVC_DESCRIPCION_GARAJE { get; set; }

            public string AVC_SALA_AREA_CONSTRUIDA { get; set; }

            public string AVC_AREA_PRIVADA { get; set; }

            public string AVC_CUADRO_VALORACION_INMUEBLE { get; set; }
        
    }
}
