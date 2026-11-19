using System.ComponentModel;


namespace GA2.Infraestructure.Data
{
    public enum EnumReintegrosParameters
    {
        [Description("@NRO_RADICADO")]
        NumeroRadicado,

        [Description("@UEJ_ID")]
        UnidadEjecutora,

        [Description("@RT_TIPO_ID")]
        TipoReintegro,

        [Description("@RT_ESTADO_ID")]
        EstadoReintegro,

        [Description("@FECHA_REINTEGRO")]
        FechaReintegro,

        [Description("@VALOR_SOLICITADO")]
        ValorSolicitado,

        [Description("@CREADO_POR")]
        CreadoPor
    }

    public enum EnumDetalleReintegrosParameters
    {

        [Description("@RT_REI_ID")]
        IdReintegro,

        [Description("@IDENTIFICACION")]
        Identificacion,

        [Description("@CREADO_POR")]
        CreadoPor
    }

    public enum EnumListadoReintegrosParameters
    {

        [Description("@NRO_RADICADO")]
        Radicado,

        [Description("@UEJ_ID")]
        UnidadEjecutora,

        [Description("@RT_TIPO_ID")]
        TipoReintegros
    }
    public enum EnumsCreateReintegro
    {
        [Description("@Id_Unidad_Ejecutora")]
        Id_Unidad_Ejecutora,

        [Description("@Unidad_Ejecutora")]
        Unidad_Ejecutora,

        [Description("@Id_Tipo_Reintegro")]
        Id_Tipo_Reintegro,

        [Description("@FechaCreacion")]
        FechaCreacion,

        [Description("@UsuarioCreacion")]
        UsuarioCreacion,

        [Description("@Estado")]
        Estado,

        [Description("@Observacion")]
        Observacion,

        [Description("@NoRadicado")]
        NoRadicado,

        [Description("@NumeroSolicitud")]
        NumeroSolicitud
    }
    public enum EnumsCreateAfiliadoReintegro
    {
        [Description("@Id_Reintregro")]
        Id_Reintregro,

        [Description("@TipoDocumentoId")]
        TipoDocumentoId,

        [Description("@NumeroDocumento")]
        NumeroDocumento,

        [Description("@Nombres")]
        Nombres,

        [Description("@Apellidos")]
        Apellidos,

        [Description("@Id_Afiliado")]
        Id_Afiliado,

        [Description("@ValorSueldo")]
        ValorSueldo,

        [Description("@EstadoAfiliao")]
        EstadoAfiliao,
        [Description("@ObservacionAfiliado")]
        ObservacionAfiliado,
        [Description("@TipoValor")]
        TipoValor,
        [Description("@FechaFin")]
        FechaFin

    }

    public enum EnumsConsecutiveReintegro
    {
        [Description("@mConsecutivo")]
        Consecutive,

    }

    public enum EnumEstadoAfiliado
    {
        VIGENCIA,
        EVALUACION


    }

    public enum EnumTipoReinegro
    {
        DEVOLUCIÓN_DE_RECURSOS = 1,
        BLOQUEOS = 2,
        DESBLOQUEOS = 3,
        INCONSISTENTES = 4
    }
    public enum EnumMensajeRepuesta
    {
        Exitoso,
    }
}
