using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class Embargo
    {
        public Guid EBA_ID { get; set; }
        public Guid ECC_ID { get; set; }
        public string EBA_RADICADO_WORK_MANAGER { get; set; }
        public DateTime EBA_FECHA_RADICADO_WORK_MANAGER { get; set; }
        public string EBA_RADICADO_JUZGADO { get; set; }
        public DateTime EBA_FECHA_RADICADO_JUZGADO { get; set; }
        public string EBA_EMAIL_RESPUESTA { get; set; }
        public string EBA_DIRECCION_RESPUESTA { get; set; }
        public string EBA_NOMBRE_DEMANDANTE { get; set; }
        public string EBA_NOMBRE_DEMANDADO { get; set; }
        public string EBA_EXPEDIENTE_BANCO_AGRARIO { get; set; }
        public string EBA_REMANENTE { get; set; }
        public string EBA_INCONSISTENCIA { get; set; }
        public string EBA_OBSERVACIONES { get; set; }
        public string EBA_ACLARACION { get; set; }
        public string EBA_RESPUESTA { get; set; }
        public double EBA_VALOR { get; set; }
        public double? ECC_VALOR { get; set; }
        public int CLI_ID { get; set; }
        public Guid EMB_ID_JUZGADO { get; set; }
        public Guid TPE_ID { get; set; }
        public Guid TIE_ID { get; set; }
        public DateTime EBA_FECHA_REGISTRO { get; set; }
        public DateTime EBA_FECHA_ACTUALIZACION { get; set; }
        public Guid EBA_CREADOPOR { get; set; }
        public Guid EBA_ACTUALIZADOPOR { get; set; }
        public int EMB_NUM_SOLICITUD { get; set; }
        public int TID_ID { get; set; }
        public string JUZGADO { get; set; }
        public string TIPO_PROCESO { get; set; }
        public string TIPO_EMBARGO { get; set; }
        public string TIE_NOMBRE { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string EBA_DIRECCION { get; set; }
        public string EBA_ESTADO_EMBARGO { get; set; }
        public string TPR_NOMBRE { get; set; }
        public string EBA_CREADOPOR_NOMBRE { get; set; }
        public string EBA_APELLIDO_CREADOPOR { get; set; }
        public string EBA_ESTADO { get; set; }

    }
    public class InsertarPagoEmbargo
    {
        public Guid EBA_ID { get; set; }
        public int TRE_ID { get; set; }
        public Guid PE_CREADOPOR { get; set; }
        public string? CLI_IDENTIFICACION { get; set; }
        public int TPI_ID { get; set; }
        public string PE_NOMBRE { get; set; } = null!;
        public decimal PE_VALOR { get; set; }
        public DateTime PE_FECHA { get; set; }
        public string? PE_OBSERVACIONES { get; set; }
        public bool PE_APROBADO { get; set; }
        public Guid? ENE_ENTIDAD_ID { get; set; }
        public string? NUMERO_CUENTA { get; set; }
        public Guid? TIE_ID { get; set; }


        public Guid? TPE_ID { get; set; }
        public string? EBA_RADICADO_WORK_MANAGER { get; set; }
        public Guid? EMB_ID_JUZGADO { get; set; }
        public string? EBA_CLI_IDENTIFICACION { get; set; }
        public string? EBA_NOMBRE_DEMANDADO { get; set; }
        public string? TPE_NOMBRE { get; set; }
        public string? CODIGO_INTERNO { get; set; }
        public string? NOMBRE { get; set; }
        public string? EBA_CREADOPOR_NOMBRE { get; set; }
        public string? EBA_APELLIDO_CREADOPOR { get; set; }
        public string? TIPO_RETENCION { get; set; }
        public string? EBA_EXPEDIENTE_BANCO_AGRARIO { get; set; }

    }
    public class ActualizarEstadoEmbargo
    {
        public Guid EBA_CREADOPOR { get; set; }
        public Guid EBA_ID { get; set; }
        public bool EBA_INCONSISTENCIA { get; set; }
        public string EBA_ACLARACION { get; set; }

    }

    public class InsertEmbargo
    {

        public Guid EBA_ID { get; set; }
        public string EBA_RADICADO_WORK_MANAGER { get; set; }
        public DateTime EBA_FECHA_RADICADO_WORK_MANAGER { get; set; }
        public string EBA_RADICADO_JUZGADO { get; set; }
        public DateTime EBA_FECHA_RADICADO_JUZGADO { get; set; }
        public string EBA_EMAIL_RESPUESTA { get; set; }
       // public string EBA_DIRECCION_RESPUESTA { get; set; }   
        public string EBA_EXPEDIENTE_BANCO_AGRARIO { get; set; }
        public string EBA_NOMBRE_DEMANDADO { get; set; }
        public bool EBA_REMANENTE { get; set; }
        public bool? EBA_INCONSISTENCIA { get; set; }
        public string EBA_OBSERVACIONES { get; set; }
        public string EBA_ACLARACION { get; set; }
        public string EBA_RESPUESTA { get; set; }
        public int CLI_ID { get; set; }
        public int TID_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public Guid EMB_ID_JUZGADO { get; set; }
        public Guid TPE_ID { get; set; }
        public Guid EBA_CREADOPOR { get; set; }
        public string EBA_PROCESO { get; set; }
        public string EBA_DIRECCION { get; set; }

        public List<BeneficiariosPagoEmbargo> BENEFICIARIOS { get; set; }

        public List<InsertEmbargoCuentaConcepto> CONCEPTOS { get; set; }

    }


    public class InsertEmbargoGestorDocumental {

        public string EBA_RADICADO_WORK_MANAGER { get; set; }
        public string EBA_RADICADO_JUZGADO { get; set; }
        public DateTime EBA_FECHA_RADICADO_JUZGADO { get; set; }
        public string EBA_NOMBRE_DEMANDADO { get; set; }
        public int CLI_ID { get; set; }
        public int TID_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public Guid EBA_CREADOPOR { get; set; }
        public string CODIGO_INTERNO { get; set; }
        public string Modulo { get; set; }


    }

    public class ConsultaEmbargosfiltro
    {
        public DateTime? FECHA_INICIAL { get; set; }
        public DateTime? FECHA_FINAL { get; set; }
        public int? TID_ID { get; set; }
        public int? Estado { get; set; }
        public int? Estados { get; set; }
        public string? CLI_IDENTIFICACION { get; set; }
        public string? EBA_RADICADO_WORK_MANAGER { get; set; }
        public Guid? UsrId { get; set; }

    }

    public class RadicadoEmbargo
    {
        public string EBA_RADICADO_WORK_MANAGER { get; set; }
        public DateTime EBA_FECHA_RADICADO_WORK_MANAGER { get; set; }
        public string EBA_RADICADO_JUZGADO { get; set; }
        public DateTime EBA_FECHA_RADICADO_JUZGADO { get; set; }
        public string EBA_EMAIL_RESPUESTA { get; set; }
        public string EBA_NOMBRE_DEMANDADO { get; set; }
        public int TID_ID { get; set; }
        public Guid EMB_ID_JUZGADO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public Guid EBA_CREADOPOR { get; set; }
        public int CLI_ID { get; set; }

    }




}
