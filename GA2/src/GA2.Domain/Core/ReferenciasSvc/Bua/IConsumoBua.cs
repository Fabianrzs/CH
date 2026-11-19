using GA2.Domain.Core.BuaServiceSvc;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Bua
{
    public interface IConsumoBua
    {
        Task<BUA> ObtenerBua(BUAConsultaBE consulta);
    }
}