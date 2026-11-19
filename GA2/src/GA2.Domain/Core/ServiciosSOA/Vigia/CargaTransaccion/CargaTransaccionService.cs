using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTransaccion;
using GA2.Transversals.Commons;
using System.Threading.Tasks;


namespace GA2.Domain.Core.Servicios.Vigia.CargaTransaccion
{
    public class CargaTransaccionService : ICargaTransaccionService
    {
        private readonly IConsumoVigiaCargaTransaccion _consumoVigiaCargaTransaccion;
        private readonly IAuditoriaServicesBusinessLogic _auditoria;
        /// <summary>
        /// contructor CargaTransaccionService
        /// <author>Cristian Gonzalez</author>
        /// <date>13/07/2021</date>
        /// </summary>
        /// <param name="consumoVigiaCargaTransaccion"></param>
        public CargaTransaccionService(IConsumoVigiaCargaTransaccion consumoVigiaCargaTransaccion,
            IAuditoriaServicesBusinessLogic auditoria)
        {
            _consumoVigiaCargaTransaccion = consumoVigiaCargaTransaccion;
            _auditoria = auditoria;
        }

        public async Task<Response<Resultado>> CargaTransaccion(CargaTransaccionRequest CargaTransaccion)
        {
            var response = new Response<Resultado>();
            response.GetCorrelation();

            //var result = await _consumoVigiaCargaTransaccion.CargaTransaccionAsync(CargaTransaccion);
            var result = await _auditoria.EjecutarServicio<CargaTransaccionRequest, Resultado>(
                 _consumoVigiaCargaTransaccion.CargaTransaccionAsync, CargaTransaccion, ReferenciaSvc.Vigia);

            response.Data = result;

            response.ReturnMessage = result != null ? ConstantesVigia.PeticionExitosa : ConstantesVigia.PeticionFallida;

            return response;
        }
    }


}
