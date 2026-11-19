using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumParamsProcesoGeneracionReporte
    {
        [Description("@PGR_ID")]
        IdProceso,

        [Description("@PGR_TIPO_REPORTE")]
        TipoReporte,

        [Description("@PGR_NOMBRE_REPORTE")]
        NombreReporte,

        [Description("@PGR_ESTADO")]
        Estado,

        [Description("@PGR_PROGRESO")]
        Progreso,

        [Description("@PGR_BLOB_CONTENEDOR")]
        BlobContenedor,

        [Description("@PGR_BLOB_RUTA_ACCESO")]
        BlobRutaAcceso,

        [Description("@PGR_CREADO_POR")]
        CreadoPor,

        [Description("@PGR_DETALLE_PROCESO")]
        DetalleProceso,

        [Description("@CONCATENAR_DETALLE")]
        ConcatenarDetalle,
    }
}