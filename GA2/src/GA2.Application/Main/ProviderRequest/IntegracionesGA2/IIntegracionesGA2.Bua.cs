using GA2.Application.Dto;
using System;
using System.Threading.Tasks;

namespace GA2.Application.Main
{
    public interface IIntegracionesGA2Bua
    {
        Task<object> ObtenetBua<TOutput>(Uri url, ObtenerBua buaDto, Guid correlationId, Guid userId);
    }
}
