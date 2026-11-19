using GA2.Domain.Core.ReferenciasSvc.Haberes;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Haberes
{
    /// <summary>
    /// Servicio de haberes
    /// </summary>
    /// <author>OScar Julian Rojas</author>
    /// <date>30/06/2021</date>
    public class HaberesService : IHaberesService
    {
        /// <summary>
        /// Instancia del consumo haaberes
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>30/06/2021</date>
        private readonly IConsumoHaberes _haberes;

        private readonly IAuditoriaServicesBusinessLogic _auditoria; 

        /// <summary>
        /// Construnctor de servicio haberes
        /// </summary>
        /// <param name="haberes">Instancia de consumo haberees</param>
        public HaberesService(IConsumoHaberes haberes, IAuditoriaServicesBusinessLogic auditoria)
        {
            _haberes = haberes;
            _auditoria = auditoria;
        }

        /// <summary> 
        /// Metodo generar reporte de haberes Version 2
        /// </summary>
        /// <param name="reporte">Reporte a generar</param>
        /// <returns></returns>
        public async Task<Response<string>> GenerarReporteHaberesV2(GenerarReporteV2Request reporte)
        {
            var response = new Response<string>();
            //response.Data = await _haberes.GenerarReporteHaberesV2(reporte);
            response.Data = await _auditoria.EjecutarServicio<GenerarReporteV2Request,string>(
                _haberes.GenerarReporteHaberesV2, reporte, ReferenciaSvc.Haberes);
            response.ReturnMessage = string.IsNullOrEmpty(response.Data) ? ConstantesHaberes.PeticionExitosa : ConstantesHaberes.PeticionFallida;

            return response;
        }

        /// <summary> 
        /// Metodo generar reporte de haberes Version 2
        /// </summary>
        /// <param name="reporte">Reporte a generar</param>
        /// <returns></returns>
        public async Task<Response<string>> GenerarReporteHaberes(GenerarReporteRequest reporte)
        {
            var response = new Response<string>();
            //response.Data = await _haberes.GenerarReporteHaberes(reporte);
            response.Data = await _auditoria.EjecutarServicio<GenerarReporteRequest, string>(
                _haberes.GenerarReporteHaberes, reporte, ReferenciaSvc.Haberes);
            response.ReturnMessage = !string.IsNullOrEmpty(response.Data) ? ConstantesHaberes.PeticionExitosa : ConstantesHaberes.PeticionFallida;

            return response;
        }

        /// <summary> 
        /// Metodo generar reporte de haberes Version 2
        /// </summary>
        /// <param name="reporte">Reporte a generar</param>
        /// <returns></returns>
        public async Task<Response<ReporteHaberesResponse>> ReporteHaberes(ReporteHaberesRequest reporteHaberes)
        {
            var response = new Response<ReporteHaberesResponse>();
            //response.Data = await _haberes.GenerarReporteHaberes(reporte);
            response.Data = await _auditoria.EjecutarServicio<ReporteHaberesRequest, ReporteHaberesResponse>(
                _haberes.ReporteHaberes, reporteHaberes, ReferenciaSvc.Haberes);
            response.ReturnMessage = "ok";

            return response;
        }
    }
}
