using GA2.Application.Dto.CronogramaActividades;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CronogramaNovedades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICronogramaRepository
    {
        Task<IEnumerable<Domain.Entities.Cronograma>> ObtenerUnidadesEjecutorasCronograma();
        Task<IEnumerable<Domain.Entities.Cronograma>> EditarFechaReporteCronograma(Domain.Entities.Cronograma cronograma);
        Task<IEnumerable<Domain.Entities.Cronograma>> EliminarUnidadEjecutoraCronograma(int idCronograma);
        Task<IEnumerable<Domain.Entities.Cronograma>> AgregarUnidadEjecutoraCronograma(Domain.Entities.Cronograma cronograma);
        Task<IEnumerable<Domain.Entities.NovedadesCronograma>> ObtenerNovedadesCronograma(int idUnidadFuerza);
        Task<Domain.Entities.NovedadesCronograma> InsertarNovedadCronograma(Domain.Entities.NovedadesCronograma novedad);
        Task<Domain.Entities.NovedadesCronograma> EditarNovedadCronograma(Domain.Entities.NovedadesCronograma novedad);
        Task<Domain.Entities.NovedadesCronograma> EliminarNovedadCrucigrama(int idNovedad);
        Task<NovedadesAfiliado> InsertarNovedadAfiliado(NovedadesAfiliado request);
        Task<IEnumerable<NovedadesAfiliado>> ObtenerNovedadesAfiliado(int idTipoIdentificacion, string identificacion);
        Task<IEnumerable<NovedadPorUnidadEjecutora>> ObtenerNovedadesPorUnidadEjecutora(NovedadPorUnidadEjecutora request);
        Task<NovedadesCronograma> EliminarNovedadAfiliado(int idUnidadEjecutora);
        Task<IEnumerable<GeneracionArchivos>> GeneracionArchivoNovedades(int IdUnidadEjecutora, int IdConceptoNovedad, DateTime fechaInicio, DateTime fechaFin, bool esAlta, string codigo, bool DHorizontal);
        Task<NovedadesAfiliado> ObtenerNovedadAfiliadoPorId(int i);
        Task<NovedadesAfiliado> EditarNovedadAfiliado(NovedadesAfiliado request);
        Task<IEnumerable<DescargaDefinitiva>> InsertarDescargaDefinitiva(DescargaDefinitiva descargaDefinitiva);
        Task<IEnumerable<DescargaDefinitiva>> ActualizarEstadoDescargaDefinitiva(DescargaDefinitiva descargaDefinitiva);
        Task<IEnumerable<DescargaDefinitiva>> ObtenerArchivosDescargaDefinitiva(DescargaDefinitiva descargaDefinitiva);
    }
}
