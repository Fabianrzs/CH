using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumMedioCreditoVivienda
    {
        [Description("@MCV_ID")]
        MCV_ID,
        [Description("@MCV_DESCRIPCION")]
        MCV_DESCRIPCION,
        [Description("@MCV_ORDER")]
        MCV_ORDER,
        [Description("@MCV_ESTADO")]
        MCV_ESTADO,
        [Description("@MCV_FECHA_CREACION")]
        MCV_FECHA_CREACION
    }
}
