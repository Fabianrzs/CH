using GA2.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICuentaWorkerRepository
    {
        Task<IEnumerable<CuentaDto>> ParametrosCreacionCuenta(IEnumerable<CuerpoArchivoDto> cuerpo);
        Task<IEnumerable<CuentaDto>> ParametrosCreacionMovimientos(IEnumerable<CuerpoArchivoDto> cuerpo);
        Task<IEnumerable<CuerpoArchivoDto>> CreacionCuentaCargueNomina(EncabezadoArchivoDto encabezado, IEnumerable<CuerpoArchivoDto> cuerpo, FinDeArchivoDto fin, NombreArchivoDto nombreArchivo);
    }
}