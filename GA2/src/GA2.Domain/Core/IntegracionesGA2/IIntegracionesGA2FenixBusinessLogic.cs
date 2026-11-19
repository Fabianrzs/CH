using GA2.Transversals.Commons;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IIntegracionesGA2FenixBusinessLogic
    {
        Task<object> ValidarBiometria(Uri url, int request, Guid userId);
    }
}