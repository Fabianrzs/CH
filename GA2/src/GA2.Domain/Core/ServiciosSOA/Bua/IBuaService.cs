using GA2.Domain.Core.BuaServiceSvc;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Bua
{
    public interface IBuaService
    {
        /// <summary>
        /// Metodo que busca un afiliado en las base datos de cajahonor
        /// por tipo de documento e identificacion
        /// </summary>
        /// <param name="afiliado">Afiliado que se desea buscar</param>
        /// <returns>Afiliado existente en cajahonor</returns>
        /// <author>Oscar Julian Rojas</author>
        /// <date>21/06/2021</date>
        Task<Response<BUA>> ObtenerAfiliadoBuaCH(BUAConsultaBE afiliado);
    }
}