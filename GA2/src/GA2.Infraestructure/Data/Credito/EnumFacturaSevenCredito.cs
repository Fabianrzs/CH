using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumFacturaSevenCredito
    {
        [Description("@FAC_TIPO_DOCUMENTO")]
        TipoDocumento,

        [Description("@SOC_ID")]
        IdSolicitudCredito,

        [Description("@FAC_NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@FAC_NOMBRE_AFILIADO")]
        NombreAfiliado,

        [Description("@FAC_SEGUNDO_NOMBRE_AFILIADO")]
        SegundoNombreAfiliado,

        [Description("@FAC_APELLIDO_AFILIADO")]
        ApellidoAfiliado,

        [Description("@@FAC_SEGUNDO_APELLIDO_AFILIADO")]
        SegundoApellidoAfiliado,

        [Description("@FAC_FECHA_REGISTRO_TERCERO")]
        FechaTercero,

        [Description("@FLUJO")]
        Flujo,

        [Description("@FAC_TIPO_DOCUMENTO_VENDEDOR")]
        TipoDocumentoVendedor,

        [Description("@FAC_NUMERO_DOCUMENTO_VENDEDOR")]
        NumeroDocumentoVendedor,

        [Description("@FAC_NOMBRE_COMPLETO_VENDEDOR")]
        NombreCompletoVendedor,

        [Description("@FAC_TIPO_CUENTA_VENDEDOR")]
        TipoCuentaVendedor,

        [Description("@FAC_NUMERO_CUENTA_VENDEDOR")]
        NumeroCuentaVendedor,

        [Description("@FAC_BANCO_VENDEDOR")]
        BancoVendedor,

        [Description("@FAC_FECHA_REGISTRO_VENDEDOR")]
        FechaVendedor,

        [Description("@FAC_CDP")]
        CDP,

        [Description("@FAC_TIPO_CREDITO")]
        TipoCredito,

        [Description("@FAC_VALOR_DESEMBOLSO")]
        ValorDesembolso,

        [Description("@FAC_RADICADO")]
        Radicado,

        [Description("@FAC_AREA_NEGOCIO")]
        AreaNegocio,

        [Description("@FAC_FECHA_REGISTRO_CDP")]
        FechaCDP,

        [Description("@FAC_NUMERO_FACTURA")]
        NumeroFactura,

        [Description("@FAC_FECHA_REGISTRO_FACTURA")]
        FechaFactura,


    }
}
