using System;
using System.Threading.Tasks;

namespace GA2.Application.Main
{
    public interface IIntegracionesGA2Fenix
    {
        Task<object> ValidarBiometria<TOutput>(Uri url, int request, Guid correlationId, Guid userId);
    }
}