using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Fenix;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaProductoTercero;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Vigia.CargaProductoTercero
{
    public class CargaProductoTerceroService : ICargaProductoTerceroService
    {
        private readonly IConsumoVigiaCargaProductoTercero _consumoVigiaCargaProductoTercero;
        private readonly IAuditoriaServicesBusinessLogic _auditoria;
        /// <summary>
        /// Constructor de CargaProductoTerceroService
        /// <author>Cristian Gonzalez</author>
        /// <date>13/07/2021</date>
        /// </summary>
        /// <param name="consumoVigiaCargaProductoTercero"></param>
        public CargaProductoTerceroService(IConsumoVigiaCargaProductoTercero consumoVigiaCargaProductoTercero,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _consumoVigiaCargaProductoTercero = consumoVigiaCargaProductoTercero;
            _auditoria = auditoria;
        }

        public async Task<Response<Resultado>> CargaProductoTercero(CargaProductoTerceroRequest cargaProductoTercero)
        {
            var response = new Response<Resultado>();
            response.GetCorrelation();
            //var result = await _consumoVigiaCargaProductoTercero.CargaProductoTerceroAsync(cargaProductoTercero);
            var result = await _auditoria.EjecutarServicio<CargaProductoTerceroRequest, Resultado>(
                 _consumoVigiaCargaProductoTercero.CargaProductoTerceroAsync, cargaProductoTercero, ReferenciaSvc.Vigia);
            response.Data = result;
            response.ReturnMessage = result != null ? ConstantesVigia.PeticionExitosa : ConstantesVigia.PeticionFallida;
            return response;
        }
    }
}
