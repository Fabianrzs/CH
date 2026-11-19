using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Cronograma
{
    public enum EnumDescargaDefinitiva
    {
        [Description("@NVA_DES_ID")]
        NVA_DES_ID,
        [Description("@NOMBRE_ARCHIVO")]
        NOMBRE_ARCHIVO,
        [Description("@PERIODO")]
        PERIODO,
        [Description("@UEJ_ID")]
        UEJ_ID,
        [Description("@CNV_ID")]
        CNV_ID,
        [Description("@USUARIO")]
        USUARIO,
        [Description("@NVA_DES_ESTADO")]
        NVA_DES_ESTADO,
        [Description("@NVA_DESCARGADO")]
        NVA_DESCARGADO
    }
}
