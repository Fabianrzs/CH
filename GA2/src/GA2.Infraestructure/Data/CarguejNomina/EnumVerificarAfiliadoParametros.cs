using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data
{
    /// <summary>
    /// Enum de un procedimiento almacenado de mombre VerificarAfiliado
    /// </summary>
    /// <author>Cristian Gonzalez</author>
    /// <date>09/08/2021</date>
    public enum EnumVerificarAfiliadoParametros
    {
        [Description("CTA_ID")]
		CTA_ID,
		[Description("CTA_NUMERO")]
		CTA_NUMERO,
		[Description("CLI_ID")]
		CLI_ID,
		[Description("TCT_ID")]
		TCT_ID,
		[Description("ECT_ID")]
		ECT_ID,
		[Description("CCN_ID")]
		CCN_ID,
		[Description("DCT_ID")]
		DCT_ID,
		[Description("CTA_FECHA_CREACION")]
		CTA_FECHA_CREACION,
		[Description("CTA_FECHA_CIERRE")]
		CTA_FECHA_CIERRE,
		[Description("CTA_FECHA_PRIMER_APORTE")]
		CTA_FECHA_PRIMER_APORTE,
		[Description("CTA_SALDO")]
		CTA_SALDO,
		[Description("CTA_SALDO_CANJE")]
		CTA_SALDO_CANJE,
		[Description("CTA_SALDO_DISPONIBLE")]
		CTA_SALDO_DISPONIBLE,
		[Description("CTA_CUOTAS")]
		CTA_CUOTAS,
		[Description("MVT_ID")]
		MVT_ID,
		[Description("CNC_ID")]
		CNC_ID,
		[Description("MVT_VALOR")]
		MVT_VALOR,
		[Description("CAT_TIPO_MOVIMIENTO")]
		CAT_TIPO_MOVIMIENTO,
		[Description("MVT_FECHA_PROCESO")]
		MVT_FECHA_PROCESO,
		[Description("MVT_ANO_PERIODO")]
		MVT_ANO_PERIODO,
		[Description("MVT_FECHA_MOVIMIENTO")]
		MVT_FECHA_MOVIMIENTO

    }



	public enum CamposInformacionAfiliadoCargueNomina
	{
		[Description("@ICN_ID")]
		ICN_ID,

		[Description("@ICN_AFILIADO")]
		ICN_AFILIADO,

        [Description("@ICN_NUMERO_DOCUMENTO")]
        ICN_NUMERO_DOCUMENTO,

        [Description("@ICN_UNIDAD_EJECUTORA")]
        ICN_UNIDAD_EJECUTORA,

        [Description("@ICN_TIPO_CUENTA")]
        ICN_TIPO_CUENTA,

        [Description("@ICN_ESTADO_CUENTA")]
        ICN_ESTADO_CUENTA,

        [Description("@ICN_AHORROS_OBLIGATORIOS")]
        ICN_AHORROS_OBLIGATORIOS,

        [Description("@ICN_AHORROS_VOLUNTARIOS")]
        ICN_AHORROS_VOLUNTARIOS,

        [Description("@ICN_CESANTIAS")]
        ICN_CESANTIAS,

        [Description("@ICN_AHORROS_RETROACTIVOS")]
        ICN_AHORROS_RETROACTIVOS,

        [Description("@ICN_CESANTIAS_RETROACTIVOS")]
        ICN_CESANTIAS_RETROACTIVOS,

        [Description("@ICN_INTERESES_APORTES")]
        ICN_INTERESES_APORTES,

        [Description("@ICN_INTERESES_CESANTIAS")]
        ICN_INTERESES_CESANTIAS,

        [Description("@ICN_CESANTIAS_DOCEAVAS")]
        ICN_CESANTIAS_DOCEAVAS,

        [Description("@ICN_TOTAL_CONCEPTO")]
        ICN_TOTAL_CONCEPTO,

        [Description("@ICN_FECHA_REGISTRO")]
        ICN_FECHA_REGISTRO,

        [Description("@PRIMER_NOMBRE")]
        PrimerNombre,

        [Description("@SEGUNDO_NOMBRE")]
        SegundoNombre,
        [Description("@PRIMER_APELLIDO")]
        PrimerApellido,
        [Description("@SEGUNDO_APELLIDO")]
        SegundoApellido

    }

}
