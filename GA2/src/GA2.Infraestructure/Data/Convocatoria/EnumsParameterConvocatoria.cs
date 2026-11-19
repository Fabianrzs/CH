using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumsCreateCierreConvocatoria
    {
        [Description("@IdConvocatoria")]
        IdConvocatoria,

        [Description("@NumeroResolucionCierre")]
        NumeroResolucionCierre,

        [Description("@FechaApertura")]
        FechaApertura,

        [Description("@FechaCierre")]
        FechaCierre,

        [Description("@NumeroBeneficiarios")]
        NumeroBeneficiarios,

        [Description("@NumeroConvocatoria")]
        NumeroConvocatoria,

        [Description("@ValorOficial")]
        ValorOficial,

        [Description("@ValorSubOficial")]
        ValorSubOficial,

        [Description("@ValorSoldado")]
        ValorSoldado,

        [Description("@Acuerdo")]
        Acuerdo,

        [Description("@FechaPublicacionDiario")]
        FechaPublicacionDiario,

        [Description("@FechaNotificacion")]
        FechaNotificacion,
    }
    public enum EnumsUpdateEstadoConvocatoria
    {
        [Description("@IdConvocatoria")]
        IdConvocatoria,

        [Description("@IdEstado")]
        IdEstado,
    }
}
