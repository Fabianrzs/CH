using GA2.Domain.Entities;
using System.ComponentModel;

namespace GA2.Domain.Core.Reintegros
{
    public class EnumsTipoValorReintegro
    {
        public const string Porcenjate = "PORCENTAJE";
        public const string Valor = "Valor";
    }
    public class EnumsTipoSolicitudReintegro
    {
        public const string TipoSolicitudReintegro = "REINTEGROS UNIDADES EJECUTORAS";
    }
    public class EnumsTipoEstadoReintro
    {
        public const string Procesada = "PROCESADO";
        public const string Evaluacion = "EN EVALUACION";
    }
    public class EnumsTipoReintegro
    {
        public const int Bloqueos = 2;
    }
}
