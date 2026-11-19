using GA2.Application.Dto;
using GA2.Application.Dto.CronogramaActividades;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface ICronogramaNovedadesBusinessLogic
    {
        Task<Response<IEnumerable<CronogramaDto>>> ObtenerUnidadesEjecutorasCronograma();
        Task<Response<IEnumerable<CronogramaDto>>> EditarFechaRegistroCronograma(CronogramaDto editarCronogramaDTO);
        Task<Response<IEnumerable<CronogramaDto>>> EliminarUnidadEjecutoraCronograma(int idCronograma);
        Task<Response<NovedadesCronogramaDto>> EliminarNovedad(int idNovedad);
        Task<Response<IEnumerable<CronogramaDto>>> AgregarUnidadEjecutoraCronograma(CronogramaDto cronogramaDTO); 
        Task<Response<IEnumerable<NovedadesCronogramaDto>>> ObtenerNovedadesUnidadEjecutora(int idUnidadEjecutora);
        Task<Response<NovedadesAfiliadoDto>> InsertarNovedadAfiliado(NovedadesAfiliadoDto request);
        Task<Response<IEnumerable<NovedadesAfiliadoDto>>> ObtenerNovedadesAfiliado(int idTipoIdentificacion, string identificacion);
        Task<Response<IEnumerable<NovedadPorUnidadEjecutoraDto>>> ObtenerNovedadesPorUnidadEjecutora(NovedadPorUnidadEjecutoraDto request);
        Task<Response<NovedadesCronogramaDto>> EliminarNovedadAfiliado(int idUnidadEjecutora);
        Task<Response<GeneracionArchivosResponse>> GeneracionArchivoNovedades(GenerarArchivoRequest idUnidadEjecutora);
        Task<Response<NovedadesAfiliadoDto>> ObtenerNovedadAfiliadoPorId(int idNovedad);
        Task<Response<NovedadesAfiliadoDto>> EditarNovedadAfiliado(NovedadesAfiliadoDto request);
        Task<string> CreateReportHtml(IEnumerable<GeneracionArchivos> generacionArchivos, string html, Response<IEnumerable<UnidadEjecutoraDto>> fuerzasDto, string nameUE, GenerarArchivoRequest generarArchivoRequest);
        byte[] GeneratePdfReportFromHtml(string html);
        Task<Response<IEnumerable<DescargaDefinitivaDto>>> InsertarDescargaDefinitiva(DescargaDefinitivaDto descargaDefinitivaDto);
        Task<Response<IEnumerable<DescargaDefinitivaDto>>> ActualizarEstadoDescargaDefinitiva(DescargaDefinitivaDto descargaDefinitivaDto);
        Task<Response<IEnumerable<DescargaDefinitivaDto>>> ObtenerArchivosDescargaDefinitiva(DescargaDefinitivaDto descargaDefinitivaDto);
    }
}
