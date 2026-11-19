using GA2.Domain.Core.BuaServiceSvc;
using GA2.Domain.Core.ReferenciasSvc.Bua;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Bua
{
    /// <summary>
    /// BuaService, servicio que provee la facilidad de comunicacion con las instancia
    /// de los servicios svc de obtenerbua caja honor
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>21/06/2021</date>
    public class BuaService : IBuaService
    {
        /// <summary>
        /// Instancia de consumo bua
        /// </summary>
        private readonly IConsumoBua _consumo;

        /// <summary>
        /// Constructor Servicio Bua
        /// </summary>
        /// <param name="consumo"></param>
        public BuaService(IConsumoBua consumo)
        {
            _consumo = consumo;
        }

        /// <summary>
        /// Obtener afiliados bua
        /// </summary>
        /// <param name="afiliado">identificacion de afiliado</param>
        /// <returns>Afiliado bua CH</returns>
        /// <author>Oscar Julian Rojas</author>
        /// <date>30/06/2021</date>
        public async Task<Response<BUA>> ObtenerAfiliadoBuaCH(BUAConsultaBE afiliado)
        {
            return new Response<BUA> { Data = await _consumo.ObtenerBua(afiliado) };
        }
    }
}
