using GA2.Application.Dto;
using GA2.Application.Dto.Parametrization;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    /// <summary>
    /// lBusinessLogic de Parametrizacion 
    /// <author>Camilo Andres Yaya Poveda</author>
    /// <date>20/04/2021</date>
    /// </summary>
    public interface IParametrizacionBusinessLogic
    {
        Task<Response<ParametrizacionSeguroDto>> ObtenerSeguro();
        Task<Response<ParametrizacionTasaDto>> ObtenerTasa();
        Task<Response<ParametrizacionPlazosDto>> ObtenerPlazo();
        Task<Response<IEnumerable<ParametrizacionLegalTasaDto>>> ObtenerLegalTasa();
        Task<Response<ParametrizacionLegalAlivioDto>> ObtenerLegalAlivio();
        Task<Response<ParametrizacionLegalGmfDto>> ObtenerLegalGmf(); 
        Task<Response<ParametrizacionLiquidacionDto>> ObtenerLiquidacion();
        Task<Response<ParametrizacionSeguroDto>> CrearSeguro(ParametrizacionSeguroDto parametrizacionSeguro);
        Task<Response<ParametrizacionTasaDto>> CrearTasa(ParametrizacionTasaDto parametrizacionTasa);
        Task<Response<ParametrizacionPlazosDto>> CrearPlazo(ParametrizacionPlazosDto parametrizacionDeadLine);
        Task<Response<ParametrizacionLegalTasaDto>> CrearLegalTasa(ParametrizacionLegalTasaDto parametrizacionLegalTasa);
        Task<Response<ParametrizacionLegalAlivioDto>> CrearLegalAlivio(ParametrizacionLegalAlivioDto parametrizacionLegalAlivio);
        Task<Response<ParametrizacionLegalGmfDto>> CrearLegalGmf(ParametrizacionLegalGmfDto parametrizacionLegalGmf);
        Task<Response<ParametrizacionLiquidacionDto>> CrearLiquidacion(ParametrizacionLiquidacionDto parametrizacionLiquidacion);
        Task<Response<ParametrizacionLegalTasaDto>> ActualizarLegalTasa(ParametrizacionLegalTasaDto parametrizacionlegaltasa);
        Task<Response<ParametroTransaccionDto>> ActualizarParametroTransaccion(ParametroTransaccionDto parametroTransaccion);
        Task<Response<ParametrizacionLegalTasaDto>> EliminarLegalTasa(ParametrizacionLegalTasaDto parametrizacionLegalTasa);
        Task<Response<ParametroTransaccionDto>> CrearParametroTransaccion(ParametroTransaccionDto parametroTransaccion);
        Task<Response<IEnumerable<ParametroTransaccionDto>>> ObtenerParametrosTransaccion();
        Task<Response<IEnumerable<ParametrizacionArchivoModalidadDto>>> ObtenerParametrizacionArchivosModalidad(int tipoModalidadId); 
        Task<Response<IEnumerable<TablaParametroDto>>> ObtenerTablasParametrosPorCategoria(int categoriaId); 
        Task<Response<IEnumerable<ValorInmuebleResponseDto>>> ValorInmueble(ValorInmuebleDto request); 
        Task<Response<dynamic>> AgregarTablasParametros(dynamic data);
        Task<Response<IEnumerable<ParametrizacionArchivoEntidadEducativaDto>>> ObtenerParametrizacionArchivosEntidadEducativa(string entidadId);
        Task<Response<IPCDto>> CrearIPC(IPCDto parametrizacionIPC);
        Task<Response<IPCDto>> ProcesosIPC(IPCDto parametrizacionIPC);
        Task<Response<IPCDto>> ActualizarIPC(IPCDto parametrizacionIPC);

        Task<Response<IEnumerable<IPCDto>>> ObtenerReporteIPC(IPCDto parametroIPC);
        Task<Response<IEnumerable<IPCDto>>> ObtenerTodoReporteIPC();
        Task SincronizacionCuentas(IPCDto request);
        Task<Response<IEnumerable<ParametroIntegracionDto>>> ObtenerParametrosIntegraciones();



    }
}
