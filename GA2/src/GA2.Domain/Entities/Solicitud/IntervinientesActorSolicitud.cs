using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class IntervinientesActorSolicitud
    {
        public Guid? ITR_ID { get; set; }
        public string? ITR_TIPO_PERSONA { get; set; }

        public int? ITR_TIPO_DOCUMENTO { get; set; }
        public string? ITR_NUMERO_DOCUMENTO { get; set; }
        public string? ITR_NUMERO_DOCUMENTO_AFILIADO { get; set; }
        public int? ITR_TIPO_DOCUMENTO_AFILIADO { get; set; }
        public string? ITR_NOMBRE { get; set; }

        public string? ITR_SEGUNDONOMBRE { get; set; }
        public string? ITR_PRIMERAPELLIDO { get; set; }
        public string? ITR_SEGUNDOAPELLIDO { get; set; }

        public string? ITR_CELULAR { get; set; }
        public string? ITR_CORREO_ELECTRONICO { get; set; }

        public int? ITR_DEPARTAMENTO { get; set; }
        public int? ITR_CIUDAD { get; set; }

        public string? ITR_DIRECCION { get; set; }
        public string? ITR_BARRIO { get; set; }
        public string? ITR_TELEFONO { get; set; }

        public Guid? ITA_ID_FK { get; set; }
        public Guid? SOL_ID_FK { get; set; }
        public Guid? SOL_ID { get; set; }
        public Guid? ITR_MODIFICADOPOR { get; set; }

        public string? ITR_TIPO_CUENTA { get; set; }
        public string? ITR_NUMERO_CUENTA_BANCARIA { get; set; }
        public int? ITR_ENTIDAD_BANCARIA { get; set; } 
        public DateTime? ITR_FECHA_EXPEDICION { get; set; }

        public string? ITR_CONSTRUCTORA { get; set; }
        public string? ITR_CONVENIO_ENTIDAD { get; set; }

        public DateTime? ITR_FECHA_REGISTRO { get; set; }
        public int? ITR_ESTADO { get; set; }

        public string? SOL_RADICADO_GD { get; set; }
        public string? SOL_NUMERO_SOLICITUD { get; set; }
        public string? ITRC_MODULO { get; set; }
    }



    public class Tramite
    {
        public DateTime SOL_FECHA_SOLICITUD { get; set; }
        public decimal SOL_VALOR_A_RETIRAR { get; set; }
        public string SOL_RADICADO_GD { get; set; }
        public string PRP_NOMBRE_RAZON_SOCIAL { get; set; }
        public string SOL_CEDULA { get; set; }
        public string ITA_NOMBRE { get; set; }
        public string ESS_DESCRIPCION { get; set; }
        public string TPS_SUB_DESCRIPCION { get; set; }
        public string TIM_DESCRIPCION { get; set; }
        public int CLI_ID { get; set; }
    }
}
