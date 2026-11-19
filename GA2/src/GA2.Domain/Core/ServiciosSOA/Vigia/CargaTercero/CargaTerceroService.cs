using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTercero;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Vigia.CargaTercero
{
    public class CargaTerceroService : ICargaTerceroService
    {
        private readonly IConsumoVigiaCargaTercero _consumoVigiaCargaTercero;
        private readonly IAuditoriaServicesBusinessLogic _auditoria;
        /// <summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>13/07/2021</date>
        /// </summary>
        /// <param name="consumoVigiaCargaTercero"></param>
        public CargaTerceroService (IConsumoVigiaCargaTercero consumoVigiaCargaTercero,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _consumoVigiaCargaTercero = consumoVigiaCargaTercero;
            _auditoria = auditoria;
        }

        public async Task<Response<Resultado>>CargaTercero(CargaTerceroRequest cargaTercero)
        {
            var response = new Response<Resultado>();
            response.GetCorrelation();
            //var result = await _consumoVigiaCargaTercero.CargaTerceroAsync(cargaTercero);
            var result = await _auditoria.EjecutarServicio<CargaTerceroRequest, Resultado>(
                 _consumoVigiaCargaTercero.CargaTerceroAsync, cargaTercero, ReferenciaSvc.Vigia);
            response.Data = result;
            response.ReturnMessage = result != null ? ConstantesVigia.PeticionExitosa : ConstantesVigia.PeticionFallida;

            return response;
        }
    }
}
