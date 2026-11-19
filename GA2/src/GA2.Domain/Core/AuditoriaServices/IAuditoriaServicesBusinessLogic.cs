using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using static GA2.Domain.Core.AuditoriaServicesBusinessLogic;

namespace GA2.Domain.Core
{
    public interface IAuditoriaServicesBusinessLogic
    {
        delegate TResult MetodoServicio<T, TResult>(T parametro);

        Task<TResult> EjecutarServicio<T, TResult>(MetodoServicio<T, Task<TResult>> funcion, T parametro, ReferenciaSvc referenciaSvc);
    }

    public enum ReferenciaSvc
    {
        [Description("WorkManagent-DODO")]
        WorkManagent,

        [Description("Seven")]
        Seven,

        [Description("BUA")]
        BUA,

        [Description("Fenix")]
        Fenix,

        [Description("Haberes")]
        Haberes,

        [Description("OTP")]
        OTP,

        [Description("Vigia")]
        Vigia,

        [Description("TusDatos")]
        TusDatos,

        [Description("Dodo")]
        Dodo,

        [Description("Internal")]
        Internal


    }
}
