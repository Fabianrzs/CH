using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Vigia.VerificaTercero
{
    public class VerificacionTerceroService : IVerificacionTerceroService
    {
        private readonly IConsumoVigiaVerificacionTercero _verificacionTerceroConsumo;
        private readonly IAuditoriaServicesBusinessLogic _auditoria;
        /// <summary>
        /// Cosntructor VerificacionTerceroService
        /// <author>Cristian Gonzalez</author>
        /// <date>13/07/2021</date>
        /// </summary>
        /// <param name="verificacionTerceroConsumo"></param>
        public VerificacionTerceroService(IConsumoVigiaVerificacionTercero verificacionTerceroConsumo,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _verificacionTerceroConsumo = verificacionTerceroConsumo;
            _auditoria = auditoria; 
        }

        public async Task<Response<Resultado>> VertificacionTercero(VerificacionTerceroRequest verificacionTercero)
        {
            //return  new Response<Resultado>(await _verificacionTerceroConsumo.VertificacionTerceros(verificacionTercero));
            return new Response<Resultado>(await _auditoria.EjecutarServicio<VerificacionTerceroRequest, Resultado>(
               _verificacionTerceroConsumo.VertificacionTerceros, verificacionTercero,ReferenciaSvc.Vigia));
        }

        public async Task<Response<Resultado>> VertificacionTerceroSolicitud(VerificacionTerceroRequest verificacionTercero)
        {
            //return new Response<Resultado>(await _verificacionTerceroConsumo.VertificacionTerceroSolicitud(verificacionTercero));
            return new Response<Resultado>(await _auditoria.EjecutarServicio<VerificacionTerceroRequest, Resultado>(
               _verificacionTerceroConsumo.VertificacionTerceroSolicitud, verificacionTercero, ReferenciaSvc.Vigia));
        }
    }
}
