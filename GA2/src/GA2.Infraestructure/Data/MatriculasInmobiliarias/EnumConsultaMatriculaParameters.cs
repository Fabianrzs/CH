using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    public enum EnumConsultaMatriculaParameters
    {
        [Description("@MAI_ID")]
        MaiId,

        [Description("@MAI_NUMERO_MATRICULA")]
        MaiNumeroMatricula,

        [Description("@MAI_DIRECCION")]
        MaiDireccion,

        [Description("@MAI_FECHA_REGISTRO")]
        MaiFechaRegistro,

        [Description("@DPD_ID_FK")]
        DepartamentoId,

        [Description("@DPC_ID_FK")]
        CiudadId,

        [Description("@MAI_PRINCIPAL")]
        MaiPrincipal,

        [Description("@OFC_ID_MATRICULA")]
        Ofc_id_matricula
    }

    public enum EnumConsultaMatriculaPorNumeroMatriculaParameters
    {

        [Description("@OFC_ID_MATRICULA")]
        Ofc_id_matricula,

        [Description("@NUM_MAI_NUMERO")]
        MaiNumMatricula,

        [Description("@SOL_ID_FK")]
        SolicitudId,

        [Description("@MAI_ID")]
        MatriculaId,

        [Description("@TIPO_BLOQUEO")]
        TipoBloqueo
    }
}
